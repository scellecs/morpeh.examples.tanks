namespace Tanks.Teams {
    using GameInput;
    using Morpeh;
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
            if (nonTeamUsers.Length <= 0 || teams.Length <= 0) {
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
            var weakTeamIndex = 0;
            Filter.ComponentsBag<Team> selectedTeams = teams.Select<Team>();
            int weakTeamUserCount = selectedTeams.GetComponent(weakTeamIndex).userCount;
            for (var i = 1; i < teams.Length; i++) {
                ref Team team = ref selectedTeams.GetComponent(i);
                if (team.userCount < weakTeamUserCount) {
                    weakTeamIndex = i;
                }
            }

            return teams.GetEntity(weakTeamIndex);
        }

        public static GameUserBalanceTeamSystem Create() {
            return CreateInstance<GameUserBalanceTeamSystem>();
        }
    }
}