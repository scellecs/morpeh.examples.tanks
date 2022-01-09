namespace Tanks.Teams {
    using Morpeh;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WinDetectSystem))]
    public sealed class WinDetectSystem : UpdateSystem {
        public float winTimeScale = 0.1f;
        private Filter nonLosingTeams;

        public override void OnAwake() {
            nonLosingTeams = World.Filter.With<Team>().Without<LosingTeam>();
        }

        public override void OnUpdate(float deltaTime) {
            if (nonLosingTeams.Length > 1) {
                return;
            }

            ref Team winTeam = ref nonLosingTeams.GetEntity(0).GetComponent<Team>();
            Debug.Log($"Team {winTeam.name} wins!");
            Time.timeScale = winTimeScale;
        }

        public static WinDetectSystem Create() {
            return CreateInstance<WinDetectSystem>();
        }
    }
}