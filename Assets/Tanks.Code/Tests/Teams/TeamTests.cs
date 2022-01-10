namespace Tanks.Teams {
    using GameInput;
    using Morpeh;
    using NUnit.Framework;

    public class TeamTests : EcsTestFixture {
        protected override void InitSystems(SystemsGroup systemsGroup) {
            systemsGroup.AddSystem(GameUserBalanceTeamSystem.Create());
        }

        [Test]
        public void ShouldPlaceUserToTeam() {
            Entity userEntity = CreateUser();
            Entity teamEntity = CreateTeam();

            RunUpdateSystems(1f);

            Assert.That(userEntity.Has<InTeam>());
            Assert.That(userEntity.GetComponent<InTeam>().team, Is.EqualTo(teamEntity));
        }

        [Test]
        public void ShouldPlaceUserToWeakTeam() {
            Entity userEntity = CreateUser();
            Entity secondTeamEntity = CreateTeam();
            Entity firstTeamEntity = CreateTeam();

            SetTeamUserCount(firstTeamEntity, 5);
            RunUpdateSystems(1f);

            Assume.That(userEntity.Has<InTeam>());
            Assert.That(userEntity.GetComponent<InTeam>().team, Is.EqualTo(secondTeamEntity));
        }

        [Test]
        public void ShouldPlaceUsersToBothTeams() {
            Entity firstUserEntity = CreateUser();
            Entity secondUserEntity = CreateUser();
            Entity firstTeamEntity = CreateTeam();
            Entity secondTeamEntity = CreateTeam();

            RunUpdateSystems(1f);
            Assume.That(firstUserEntity.Has<InTeam>());
            Assume.That(secondUserEntity.Has<InTeam>());

            ref Team firstTeam = ref firstTeamEntity.GetComponent<Team>();
            ref Team secondTeam = ref secondTeamEntity.GetComponent<Team>();
            Assert.That(firstTeam.userCount, Is.EqualTo(secondTeam.userCount));

            Entity firstUserTeamEntity = firstUserEntity.GetComponent<InTeam>().team;
            Assert.That(secondUserEntity.GetComponent<InTeam>().team, Is.Not.EqualTo(firstUserTeamEntity));
        }

        private Entity CreateUser() {
            Entity userEntity = testWorld.CreateEntity();
            userEntity.AddComponent<GameUser>();
            return userEntity;
        }

        private Entity CreateTeam() {
            Entity teamEntity = testWorld.CreateEntity();
            teamEntity.AddComponent<Team>();
            return teamEntity;
        }

        private static void SetTeamUserCount(Entity teamEntity, int count) {
            ref Team team = ref teamEntity.GetComponent<Team>();
            team.userCount = count;
        }
    }
}