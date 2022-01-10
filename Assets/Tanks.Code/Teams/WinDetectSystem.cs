namespace Tanks.Teams {
    using Morpeh;
    using Morpeh.Helpers;
    using UnityEngine;
    using UtilSystems;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WinDetectSystem))]
    public sealed class WinDetectSystem : UpdateSystem {
        public float winTimeScale = 0.1f;
        public TextInWorldSystem.Request winMessageRequest;

        private Filter nonLosingTeams;
        private Filter winMarkers;

        public override void OnAwake() {
            nonLosingTeams = World.Filter.With<Team>().Without<LosingTeam>();
            winMarkers = World.Filter.With<WinMarker>();
        }

        public override void OnUpdate(float deltaTime) {
            if (!winMarkers.IsEmpty()) {
                return;
            }

            if (nonLosingTeams.IsEmpty() || nonLosingTeams.Length > 1) {
                return;
            }

            ref Team winTeam = ref nonLosingTeams.First().GetComponent<Team>();
            var text = $"Team {winTeam.name} wins!";
            Debug.Log(text);

            TextInWorldSystem.Request request = winMessageRequest;
            request.color = winTeam.color;
            request.text = text;
            World.CreateEntity().SetComponent(request);

            Time.timeScale = winTimeScale;
            World.CreateEntity().AddComponent<WinMarker>();
        }

        public static WinDetectSystem Create() {
            return CreateInstance<WinDetectSystem>();
        }

        public struct WinMarker : IComponent { }
    }
}