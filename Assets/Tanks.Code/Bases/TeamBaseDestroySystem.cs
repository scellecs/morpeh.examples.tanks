namespace Tanks.Bases {
    using Healthcare;
    using Morpeh;
    using Teams;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TeamBaseDestroySystem))]
    public sealed class TeamBaseDestroySystem : UpdateSystem {
        public Color destroyedColor = Color.black;

        private Filter destroyedBases;

        public override void OnAwake() {
            destroyedBases = World.Filter.With<TeamBase>().With<InTeam>().With<IsDeadMarker>();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity ent in destroyedBases) {
                Entity team = ent.GetComponent<InTeam>().team;
                if (team.Has<LosingTeamMarker>()) {
                    continue;
                }

                team.AddComponent<LosingTeamMarker>();
                ent.GetComponent<TeamBase>().view.SetColor(destroyedColor);
            }
        }

        public static TeamBaseDestroySystem Create() {
            return CreateInstance<TeamBaseDestroySystem>();
        }
    }
}