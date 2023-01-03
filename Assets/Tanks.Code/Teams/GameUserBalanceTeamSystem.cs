namespace Tanks.Teams {
    using GameInput;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GameUserBalanceTeamSystem))]
    public sealed class GameUserBalanceTeamSystem : UpdateSystem {
        private Filter nonTeamUsers;
        private Filter teams;

        public override void OnAwake() {
            teams = World.Filter.With<Team>();
            nonTeamUsers = World.Filter.With<GameUser>().Without<InTeam>();
        }

        public override void OnUpdate(float deltaTime) {
            if (nonTeamUsers.IsEmpty() || teams.IsEmpty()) {
                return;
            }

            foreach (Entity ent in nonTeamUsers) {
                Entity weakTeamEntity = GetWeakTeam();
                ref Team weakTeam = ref weakTeamEntity.GetComponent<Team>();
                ent.SetComponent(new InTeam {
                        team = weakTeamEntity,
                });

                weakTeam.userCount++;
            }
        }

        private Entity GetWeakTeam() {
            Entity weakTeam = null;
            var weakTeamUserCount = int.MaxValue;

            foreach (Entity entity in teams) {
                ref Team team = ref entity.GetComponent<Team>();
                if (team.userCount < weakTeamUserCount) {
                    weakTeam = entity;
                    weakTeamUserCount = team.userCount;
                }
            }

            return weakTeam;
        }

        public static GameUserBalanceTeamSystem Create() {
            return CreateInstance<GameUserBalanceTeamSystem>();
        }
    }
}