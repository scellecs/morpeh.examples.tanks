using Morpeh;
using NUnit.Framework;
using Tanks.GameInput;

namespace Tanks.Teams {
	public class TeamTests : EcsTestFixture {

		protected override void InitSystems(SystemsGroup systemsGroup) {
			systemsGroup.AddSystem(GameUserBalanceTeamSystem.Create());
		}

		[Test]
		public void ShouldPlaceUserToTeam() {
			var userEntity = CreateUser();
			var teamEntity = CreateTeam();

			RunUpdateSystems(1f);

			Assert.That(userEntity.Has<InTeam>());
			Assert.That(userEntity.GetComponent<InTeam>().team, Is.EqualTo(teamEntity));
		}

		[Test]
		public void ShouldPlaceUserToWeakTeam() {
			var userEntity = CreateUser();
			var secondTeamEntity = CreateTeam();
			var firstTeamEntity = CreateTeam();

			SetTeamUserCount(firstTeamEntity, 5);
			RunUpdateSystems(1f);

			Assume.That(userEntity.Has<InTeam>());
			Assert.That(userEntity.GetComponent<InTeam>().team, Is.EqualTo(secondTeamEntity));
		}

		[Test]
		public void ShouldPlaceUsersToBothTeams() {
			var firstUserEntity = CreateUser();
			var secondUserEntity = CreateUser();
			var firstTeamEntity = CreateTeam();
			var secondTeamEntity = CreateTeam();

			RunUpdateSystems(1f);
			Assume.That(firstUserEntity.Has<InTeam>());
			Assume.That(secondUserEntity.Has<InTeam>());

			ref var firstTeam = ref firstTeamEntity.GetComponent<Team>();
			ref var secondTeam = ref secondTeamEntity.GetComponent<Team>();
			Assert.That(firstTeam.userCount, Is.EqualTo(secondTeam.userCount));

			var firstUserTeamEntity = firstUserEntity.GetComponent<InTeam>().team;
			Assert.That(secondUserEntity.GetComponent<InTeam>().team, Is.Not.EqualTo(firstUserTeamEntity));
		}

		private Entity CreateUser() {
			var userEntity = testWorld.CreateEntity();
			userEntity.AddComponent<GameUser>();
			return userEntity;
		}

		private Entity CreateTeam() {
			var teamEntity = testWorld.CreateEntity();
			teamEntity.AddComponent<Team>();
			return teamEntity;
		}

		private static void SetTeamUserCount(Entity teamEntity, int count) {
			ref var team = ref teamEntity.GetComponent<Team>();
			team.userCount = count;
		}
	}
}