namespace Tanks.Bases {
    using Morpeh;
    using Teams;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TeamBaseInitSystem))]
    public sealed class TeamBaseInitSystem : UpdateSystem {
        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<TeamBase>().With<InTeam>().Without<Initialized>();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity ent in filter) {
                Entity teamEntity = ent.GetComponent<InTeam>().team;
                Color teamColor = teamEntity.GetComponent<Team>().color;
                ent.GetComponent<TeamBase>().view.SetColor(teamColor);
            }
        }

        public static TeamBaseInitSystem Create() {
            return CreateInstance<TeamBaseInitSystem>();
        }
    }
}