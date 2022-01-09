namespace Tanks.Bases {
    using Healthcare;
    using Morpeh;
    using Teams;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TeamBaseDestroySystem))]
    public sealed class TeamBaseDestroySystem : UpdateSystem {
        private Filter destroyedBases;

        public override void OnAwake() {
            destroyedBases = World.Filter.With<TeamBase>().With<InTeam>().With<IsDead>();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity ent in destroyedBases) {
                Entity teamEntity = ent.GetComponent<InTeam>().team;
                teamEntity.AddComponent<LosingTeam>();
                ent.GetComponent<TeamBase>().view.SetColor(Color.black);
            }
        }

        public static TeamBaseDestroySystem Create() {
            return CreateInstance<TeamBaseDestroySystem>();
        }
    }
}