namespace Tanks.Movement {
    using Morpeh;
    using NUnit.Framework;
    using UnityEngine;

    public class MovementTests : EcsTestFixture {
        private Entity tankEntity;
        private TankConfig tankConfig;

        [SetUp]
        public void SetUp() {
            CreateTank();
        }

        protected override void InitSystems(SystemsGroup systemsGroup) {
            systemsGroup.AddSystem(MovementSystem.Create());
        }

        [Test]
        [TestCase(1f, 1f, ExpectedResult = 1f)]
        [TestCase(2f, 0.5f, ExpectedResult = 1f)]
        public float ShouldMoveTank(float speed, float dt) {
            tankConfig.speed = speed;
            Vector2 direction = Vector2.up;
            ref MoveDirection moveDirection = ref tankEntity.GetComponent<MoveDirection>();
            moveDirection.direction = direction;

            RunFixedSystems();
            SimulatePhysics(dt);

            ref Tank tank = ref tankEntity.GetComponent<Tank>();
            return tank.body.position.magnitude;
        }

        [Test]
        public void ShouldRotateByDirection() {
            Vector2 direction = Vector2.right;
            ref MoveDirection moveDirection = ref tankEntity.GetComponent<MoveDirection>();
            moveDirection.direction = direction;

            RunFixedSystems();

            ref Tank tank = ref tankEntity.GetComponent<Tank>();
            Assert.That(tank.body.rotation, Is.EqualTo(-90));
        }

        [Test]
        public void ShouldRemoveAngularVelocity() {
            ref Tank tank = ref tankEntity.GetComponent<Tank>();
            tank.body.angularVelocity = 10f;

            RunFixedSystems();

            Assert.That(tank.body.angularVelocity, Is.EqualTo(0f));
        }

        [Test]
        public void ShouldNotRotateBackIfNoMovementNextFrame() {
            Vector2 direction = Vector2.right;
            ref MoveDirection moveDirection = ref tankEntity.GetComponent<MoveDirection>();
            moveDirection.direction = direction;
            RunFixedSystems();

            moveDirection.direction = Vector2.zero;
            RunFixedSystems();

            ref Tank tank = ref tankEntity.GetComponent<Tank>();
            Assert.That(tank.body.rotation, Is.EqualTo(-90));
        }

        private void CreateTank() {
            tankConfig = ScriptableObject.CreateInstance<TankConfig>();
            tankConfig.speed = 1f;

            tankEntity = testWorld.CreateEntity();
            ref Tank tank = ref tankEntity.AddComponent<Tank>();
            tank.config = tankConfig;
            tank.body = new GameObject().AddComponent<Rigidbody2D>();
            tank.body.drag = 0f;

            ref MoveDirection moveDirection = ref tankEntity.AddComponent<MoveDirection>();
            moveDirection.direction = Vector2.zero;
            RefreshFilters();
        }

        private static void SimulatePhysics(float dt) {
            PhysicsUpdateSystem.Simulate(dt);
        }
    }
}