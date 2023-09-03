namespace Tanks.Bases {
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using Teams;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TeamBaseInitSystem))]
    public sealed class TeamBaseInitSystem : UpdateSystem {
        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<TeamBase>().With<InTeam>().Without<InitializedMarker>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity ent in filter) {
                Entity teamEntity = ent.GetComponent<InTeam>().team;
                Color teamColor = teamEntity.GetComponent<Team>().color;
                ent.GetComponent<TeamBase>().view.SetColor(teamColor);
                ent.AddComponent<InitializedMarker>();
            }
        }

        public static TeamBaseInitSystem Create() {
            return CreateInstance<TeamBaseInitSystem>();
        }

        private struct InitializedMarker : IComponent { }
    }
}