using System.Collections;
using Morpeh;
using NUnit.Framework;
using Tanks.Collisions;
using Tanks.Healthcare;
using Tanks.Teams;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tanks.Weapons {
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

            var bullets = testWorld.Filter.With<Bullet>();
            Assert.That(bullets.Length, Is.EqualTo(1));

            var body = bullets.GetEntity(0).GetComponent<Bullet>().body;
            Assert.That(body.gameObject.activeInHierarchy);
        }

        [Test]
        public void ShouldIgnoreSelfColliders() {
            RegisterSystem(CollisionInitSystem.Create());
            RunFixedSystems();
            Shoot();

            PhysicsUpdateSystem.Simulate(Time.fixedDeltaTime);

            var events = testWorld.Filter.With<CollisionEvent>();
            Assert.That(events.Length, Is.EqualTo(0));
        }

        [Test]
        public void ShouldShootByDirection() {
            ref var tank = ref tankEntity.GetComponent<Tank>();
            tank.body.rotation = -90f;

            Shoot();

            var body = testWorld.Filter.With<Bullet>().GetEntity(0).GetComponent<Bullet>().body;
            Assert.That(body.rotation, Is.EqualTo(-90f));

            var velocityAngle = Vector2.SignedAngle(Vector2.up, body.velocity);
            Assert.That(velocityAngle, Is.EqualTo(-90f));
        }

        [UnityTest]
        public IEnumerator ShouldDestroyBulletAndCollisionEventOnHit() {
            RegisterSystem(CollisionCleanSystem.Create());
            Shoot();
            var bullets = testWorld.Filter.With<Bullet>();
            Assume.That(bullets.Length, Is.AtLeast(1));

            var bulletEntity = bullets.GetEntity(0);
            var body = bulletEntity.GetComponent<Bullet>().body;
            testWorld.CreateEntity().SetComponent(new CollisionEvent {
                    first = bulletEntity
            });

            RunFixedSystems();

            yield return null;

            Assert.That(bullets.Length, Is.EqualTo(0));
            Assert.That(body == null);
        }

        [Test]
        public void ShouldApplyDamageTargetOnHit() {
            Shoot();
            var targetEntity = testWorld.CreateEntity();
            var bullets = testWorld.Filter.With<Bullet>();
            Assume.That(bullets.Length, Is.AtLeast(1));

            var bulletEntity = bullets.GetEntity(0);
            testWorld.CreateEntity().SetComponent(new CollisionEvent {
                    first = bulletEntity,
                    second = targetEntity
            });

            RunFixedSystems();

            Assert.That(targetEntity.Has<DamageEvent>());

            var evt = targetEntity.GetComponent<DamageEvent>();
            Assert.That(evt.amount, Is.EqualTo(weaponConfig.bulletConfig.damage));
        }

        [Test]
        public void ShouldNotDamageSameTeamTarget() {
            var targetEntity = testWorld.CreateEntity();
            PlaceInOneTeam(tankEntity, targetEntity);

            Shoot();
            var bullets = testWorld.Filter.With<Bullet>();
            Assume.That(bullets.Length, Is.AtLeast(1));

            var bulletEntity = bullets.GetEntity(0);
            testWorld.CreateEntity().SetComponent(new CollisionEvent {
                    first = bulletEntity,
                    second = targetEntity
            });

            RunFixedSystems();

            Assert.That(!targetEntity.Has<DamageEvent>());
        }

        [Test]
        public void ShouldNotShootWhenReloading() {
            Shoot();
            var bullets = testWorld.Filter.With<Bullet>();
            Assume.That(bullets.Length, Is.AtLeast(1));

            Shoot();

            Assert.That(bullets.Length, Is.LessThan(2));
        }

        [UnityTest]
        public IEnumerator ShouldShootAfterReloading() {
            weaponConfig.reloadTime = 2 * Time.fixedDeltaTime;
            Shoot();
            var bullets = testWorld.Filter.With<Bullet>();
            Assume.That(bullets.Length, Is.AtLeast(1));

            yield return new WaitForSeconds(weaponConfig.reloadTime + 0.01f);

            Shoot();

            Assert.That(bullets.Length, Is.AtLeast(2));
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

            ref var tank = ref tankEntity.AddComponent<Tank>();
            tank.body = tankObject.AddComponent<Rigidbody2D>();

            ref var weapon = ref tankEntity.AddComponent<BulletWeapon>();
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
            ref var weapon = ref tankEntity.GetComponent<BulletWeapon>();
            weapon.shoot = true;
            RunFixedSystems();
            weapon.shoot = false;
        }

        private void PlaceInOneTeam(Entity firstEntity, Entity secondEntity) {
            var teamEntity = testWorld.CreateEntity();
            firstEntity.SetComponent(new InTeam {
                    team = teamEntity
            });

            secondEntity.SetComponent(new InTeam {
                    team = teamEntity
            });
        }
    }
}