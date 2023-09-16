namespace Tanks.Weapons {
    using System.Collections;
    using Collisions;
    using Healthcare;
    using NUnit.Framework;
    using Scellecs.Morpeh;
    using Teams;
    using UnityEngine;
    using UnityEngine.TestTools;

    public class BulletWeaponsTests : EcsTestFixture {
        private BulletWeaponConfig weaponConfig;
        private Entity tankEntity;

        protected override void InitSystems(SystemsGroup systemsGroup) {
            systemsGroup.AddSystem(BulletWeaponSystem.Create());
            systemsGroup.AddSystem(BulletHitSystem.Create());
        }

        [SetUp]
        public void SetUp() {
            CreateWeaponConfig();
            CreateTankWithWeapons();
        }

        [Test]
        public void ShouldCreateBullet() {
            Shoot();

            Filter bullets = testWorld.Filter.With<Bullet>().Build();
            Assert.That(bullets.GetLengthSlow(), Is.EqualTo(1));

            Rigidbody2D body = bullets.GetEntity(0).GetComponent<Bullet>().body;
            Assert.That(body.gameObject.activeInHierarchy);
        }

        [Test]
        public void ShouldIgnoreSelfColliders() {
            RegisterAdditionalSystems(new ISystem[] {
                    CollisionInitSystem.Create(),
            });

            RunFixedSystems();
            Shoot();

            PhysicsUpdateSystem.Simulate(Time.fixedDeltaTime);

            Filter events = testWorld.Filter.With<CollisionEvent>().Build();
            Assert.That(events.GetLengthSlow(), Is.EqualTo(0));
        }

        [Test]
        public void ShouldShootByDirection() {
            ref Tank tank = ref tankEntity.GetComponent<Tank>();
            tank.body.rotation = -90f;

            Shoot();

            Rigidbody2D body = testWorld.Filter.With<Bullet>().Build().GetEntity(0).GetComponent<Bullet>().body;
            Assert.That(body.rotation, Is.EqualTo(-90f));

            float velocityAngle = Vector2.SignedAngle(Vector2.up, body.velocity);
            Assert.That(velocityAngle, Is.EqualTo(-90f));
        }

        [UnityTest]
        public IEnumerator ShouldDestroyBulletAndCollisionEventOnHit() {
            RegisterAdditionalSystems(new ISystem[] {
                    CollisionCleanSystem.Create(),
            });

            Shoot();
            Filter bullets = testWorld.Filter.With<Bullet>().Build();
            Assume.That(bullets.GetLengthSlow(), Is.AtLeast(1));

            Entity bulletEntity = bullets.GetEntity(0);
            Rigidbody2D body = bulletEntity.GetComponent<Bullet>().body;
            testWorld.CreateEntity().SetComponent(new CollisionEvent {
                    first = bulletEntity,
            });

            RunFixedSystems();

            yield return null;

            Assert.That(bullets.GetLengthSlow(), Is.EqualTo(0));
            Assert.That(body == null);
        }

        [Test]
        public void ShouldApplyDamageTargetOnHit() {
            Shoot();
            Entity targetEntity = testWorld.CreateEntity();
            Filter bullets = testWorld.Filter.With<Bullet>().Build();
            Assume.That(bullets.GetLengthSlow(), Is.AtLeast(1));

            Entity bulletEntity = bullets.GetEntity(0);
            testWorld.CreateEntity().SetComponent(new CollisionEvent {
                    first = bulletEntity,
                    second = targetEntity,
            });

            RunFixedSystems();

            Assert.That(targetEntity.Has<DamageEvent>());

            var evt = targetEntity.GetComponent<DamageEvent>();
            Assert.That(evt.amount, Is.EqualTo(weaponConfig.bulletConfig.damage));
        }

        [Test]
        public void ShouldNotDamageSameTeamTarget() {
            Entity targetEntity = testWorld.CreateEntity();
            PlaceInOneTeam(tankEntity, targetEntity);

            Shoot();
            Filter bullets = testWorld.Filter.With<Bullet>().Build();
            Assume.That(bullets.GetLengthSlow(), Is.AtLeast(1));

            Entity bulletEntity = bullets.GetEntity(0);
            testWorld.CreateEntity().SetComponent(new CollisionEvent {
                    first = bulletEntity,
                    second = targetEntity,
            });

            RunFixedSystems();

            Assert.That(!targetEntity.Has<DamageEvent>());
        }

        [Test]
        public void ShouldNotShootWhenReloading() {
            Shoot();
            Filter bullets = testWorld.Filter.With<Bullet>().Build();
            Assume.That(bullets.GetLengthSlow(), Is.AtLeast(1));

            Shoot();

            Assert.That(bullets.GetLengthSlow(), Is.LessThan(2));
        }

        [UnityTest]
        public IEnumerator ShouldShootAfterReloading() {
            weaponConfig.reloadTime = 2 * Time.fixedDeltaTime;
            Shoot();
            Filter bullets = testWorld.Filter.With<Bullet>().Build();
            Assume.That(bullets.GetLengthSlow(), Is.AtLeast(1));

            yield return new WaitForSeconds(weaponConfig.reloadTime + 0.01f);

            Shoot();

            Assert.That(bullets.GetLengthSlow(), Is.AtLeast(2));
        }

        private void CreateWeaponConfig() {
            weaponConfig = ScriptableObject.CreateInstance<BulletWeaponConfig>();
            weaponConfig.bulletConfig = ScriptableObject.CreateInstance<BulletConfig>();
            weaponConfig.bulletConfig.prefab = CreateBulletPrefab();
            weaponConfig.bulletConfig.damage = 1f;
            weaponConfig.bulletSpeed = 1f;
            weaponConfig.reloadTime = 1f;
        }

        private void CreateTankWithWeapons() {
            tankEntity = testWorld.CreateEntity();
            var tankObject = new GameObject("Tank");
            tankObject.AddComponent<BoxCollider2D>();

            ref Tank tank = ref tankEntity.AddComponent<Tank>();
            tank.body = tankObject.AddComponent<Rigidbody2D>();

            ref BulletWeapon weapon = ref tankEntity.AddComponent<BulletWeapon>();
            weapon.config = weaponConfig;
            weapon.lastShotTime = Time.time - weapon.config.reloadTime - 0.1f;
        }

        private static Rigidbody2D CreateBulletPrefab() {
            var bulletGo = new GameObject("Bullet");
            var bulletPrefab = bulletGo.AddComponent<Rigidbody2D>();
            bulletGo.AddComponent<BoxCollider2D>();
            bulletGo.SetActive(false);
            return bulletPrefab;
        }

        private void Shoot() {
            ref BulletWeapon weapon = ref tankEntity.GetComponent<BulletWeapon>();
            weapon.shoot = true;
            RunFixedSystems();
            weapon.shoot = false;
        }

        private void PlaceInOneTeam(Entity firstEntity, Entity secondEntity) {
            Entity teamEntity = testWorld.CreateEntity();
            firstEntity.SetComponent(new InTeam {
                    team = teamEntity,
            });

            secondEntity.SetComponent(new InTeam {
                    team = teamEntity,
            });
        }
    }
}