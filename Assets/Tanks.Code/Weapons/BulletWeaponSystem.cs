namespace Tanks.Weapons {
    using Collisions;
    using Morpeh;
    using Morpeh.Helpers;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BulletWeaponSystem))]
    public sealed class BulletWeaponSystem : SimpleFixedUpdateSystem<BulletWeapon, Tank> {
        protected override void Process(Entity ent, ref BulletWeapon weapon, ref Tank tank, float dt) {
            if (!weapon.shoot) {
                return;
            }

            if (Time.time - weapon.lastShotTime < weapon.config.reloadTime) {
                return;
            }

            CreateBullet(ent, weapon, tank);
            weapon.lastShotTime = Time.time;
        }

        private void CreateBullet(Entity ent, BulletWeapon weapon, Tank tank) {
            Rigidbody2D bulletBody = Instantiate(weapon.config.bulletConfig.prefab, tank.body.position, Quaternion.identity);
            IgnoreSelfCollisions(bulletBody.GetComponent<Collider2D>(), ent);
            bulletBody.gameObject.SetActive(true);
            bulletBody.rotation = tank.body.rotation;
            bulletBody.velocity = Quaternion.Euler(0f, 0f, bulletBody.rotation) * Vector3.up * weapon.config.bulletSpeed;

            Entity bulletEntity = World.CreateEntity();
            bulletEntity.SetComponent(new Bullet {
                    body = bulletBody,
                    config = weapon.config.bulletConfig,
                    shooter = ent,
            });
        }

        private static void IgnoreSelfCollisions(Collider2D bulletCollider, Entity ent) {
            if (bulletCollider == null || !ent.Has<CanCollide>()) {
                return;
            }

            CollisionDetector detector = ent.GetComponent<CanCollide>().detector;
            foreach (Collider2D selfCollider in detector.colliders) {
                Physics2D.IgnoreCollision(selfCollider, bulletCollider);
            }
        }

        public static BulletWeaponSystem Create() {
            return CreateInstance<BulletWeaponSystem>();
        }
    }
}