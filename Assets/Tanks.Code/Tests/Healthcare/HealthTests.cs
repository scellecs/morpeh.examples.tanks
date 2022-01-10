using Morpeh;
using NUnit.Framework;

namespace Tanks.Healthcare {
	public class HealthTests : EcsTestFixture {
		private Entity testEntity;

		protected override void InitSystems(SystemsGroup systemsGroup) {
			systemsGroup.AddSystem(DamageSystem.Create());
			systemsGroup.AddSystem(DamageCleanSystem.Create());
		}

		[SetUp]
		public void SetUp() {
			testEntity = testWorld.CreateEntity();
			testEntity.AddComponent<Health>();
		}

		[Test]
		[TestCase(100f, 0f, ExpectedResult = 100f)]
		[TestCase(100f, 10f, ExpectedResult = 90f)]
		[TestCase(100f, 50f, ExpectedResult = 50f)]
		public float ShouldReduceHealth(float startHealth, float damage) {
			testEntity.SetComponent(new Health {
				health = startHealth
			});

			testEntity.SetComponent(new DamageEvent {
				amount = damage
			});
			RunUpdateSystems(1f);

			return testEntity.GetComponent<Health>().health;
		}

		[Test]
		public void ShouldMarkAsDead() {
			testEntity.SetComponent(new Health {
				health = 10f
			});

			testEntity.SetComponent(new DamageEvent {
				amount = 11f
			});
			RunUpdateSystems(1f);

			Assert.That(testEntity.Has<IsDeadMarker>());
		}

		[Test]
		public void ShouldCleanDamageEventsOnLateUpdate() {
			testEntity.AddComponent<DamageEvent>();

			RunLateUpdateSystems(1f);

			Assert.That(!testEntity.Has<DamageEvent>());
		}
	}
}