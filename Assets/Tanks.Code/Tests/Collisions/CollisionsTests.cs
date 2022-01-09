using Morpeh;
using NUnit.Framework;
using Tanks.Walls;
using Tanks.Weapons;
using UnityEngine;

namespace Tanks.Collisions {
    public class CollisionsTests : EcsTestFixture {
        protected override void InitSystems(SystemsGroup systemsGroup) {
            systemsGroup.AddSystem(CollisionInitSystem.Create());
            systemsGroup.AddSystem(CollisionCleanSystem.Create());
        }

        [Test]
        public void ShouldMakeTankCanCollide() {
            var tankEntity = CreateTank();

            RunFixedSystems();

            Assert.That(tankEntity.Has<CanCollide>());
        }

        [Test]
        public void ShouldMakeBulletCanCollide() {
            var bulletEntity = CreateBullet();

            RunFixedSystems();

            Assert.That(bulletEntity.Has<CanCollide>());
        }

        [Test]
        public void ShouldMakeWallCanCollide() {
            var wallEntity = CreateWall();

            RunFixedSystems();

            Assert.That(wallEntity.Has<CanCollide>());
        }

        [Test]
        public void ShouldSendCollisionEventWhenSecondIsNotEntity() {
            var tankEntity = CreateTank();
            RunFixedSystems();
            CreateBox();

            SimulatePhysics(Time.fixedDeltaTime);

            var events = testWorld.Filter.With<CollisionEvent>();
            Assert.That(events.Length, Is.AtLeast(1));

            var evtEntity = events.GetEntity(0);
            var evt = evtEntity.GetComponent<CollisionEvent>();
            Assert.That(evt.first, Is.EqualTo(tankEntity));
            Assert.That(evt.second, Is.Null);
        }

        [Test]
        public void ShouldSendCollisionEventsWhenSecondIsEntity() {
            CreateTank();
            CreateTank();
            RunFixedSystems();

            SimulatePhysics(Time.fixedDeltaTime);

            var events = testWorld.Filter.With<CollisionEvent>();
            Assert.That(events.Length, Is.AtLeast(2));

            foreach (var evtEnt in events) {
                var evt = evtEnt.GetComponent<CollisionEvent>();
                Assert.That(evt.first, Is.Not.Null);
                Assert.That(evt.second, Is.Not.Null);
                Assert.That(evt.first, Is.Not.EqualTo(evt.second));
            }
        }

        [Test]
        public void ShouldCleanCollisionsInLateUpdate() {
            testWorld.CreateEntity().SetComponent(new CollisionEvent());

            RunLateUpdateSystems(1f);

            var events = testWorld.Filter.With<CollisionEvent>();
            Assert.That(events.Length, Is.EqualTo(0));
        }

        private Entity CreateTank() {
            var tankEntity = testWorld.CreateEntity();
            var tankGo = new GameObject("Tank");
            tankGo.AddComponent<BoxCollider2D>();
            tankEntity.SetComponent(new Tank {
                    body = tankGo.AddComponent<Rigidbody2D>()
            });

            return tankEntity;
        }

        private Entity CreateBullet() {
            var bulletEntity = testWorld.CreateEntity();
            var bulletGo = new GameObject("Bullet");
            bulletGo.AddComponent<BoxCollider2D>();
            bulletEntity.SetComponent(new Bullet {
                    body = bulletGo.AddComponent<Rigidbody2D>()
            });

            return bulletEntity;
        }

        private Entity CreateWall() {
            var wallEntity = testWorld.CreateEntity();
            var wallGo = new GameObject("Wall");
            wallGo.AddComponent<BoxCollider2D>();
            wallEntity.SetComponent(new Wall {
                    transform = wallGo.transform
            });

            return wallEntity;
        }

        private static Collider2D CreateBox() {
            return new GameObject("Box").AddComponent<BoxCollider2D>();
        }

        private static void SimulatePhysics(float dt) {
            PhysicsUpdateSystem.Simulate(dt);
        }
    }
}