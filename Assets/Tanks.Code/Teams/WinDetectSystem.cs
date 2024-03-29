﻿namespace Tanks.Teams {
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using UnityEngine;
    using UtilSystems;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WinDetectSystem))]
    public sealed class WinDetectSystem : UpdateSystem {
        public float winTimeScale = 0.1f;
        public TextInWorldSystem.Request winMessageRequest;

        private Filter nonLosingTeams;
        private Filter winMarkers;

        public override void OnAwake() {
            nonLosingTeams = World.Filter.With<Team>().Without<LosingTeamMarker>().Build();
            winMarkers = World.Filter.With<WinMarker>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            if (!winMarkers.IsEmpty()) {
                return;
            }

            if (nonLosingTeams.IsEmpty() || nonLosingTeams.GetLengthSlow() > 1) {
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