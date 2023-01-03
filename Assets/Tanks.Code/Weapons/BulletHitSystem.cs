namespace Tanks.Weapons {
    using Collisions;
    using Healthcare;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Helpers;
    using Teams;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BulletHitSystem))]
    public sealed class BulletHitSystem : SimpleFixedUpdateSystem<CollisionEvent> {
        protected override void Process(Entity ent, ref CollisionEvent evt, in float dt) {
            Entity bulletEntity = evt.first;
            ref Bullet bullet = ref bulletEntity.GetComponent<Bullet>(out bool isBullet);
            if (!isBullet) {
                return;
            }

            if (evt.second != null && !evt.second.InSameTeam(bullet.shooter)) {
                evt.second.SetComponent(new DamageEvent {
                        hitPosition = evt.collision?.GetContact(0).point,
                        amount = bullet.config.damage,
                        dealer = bullet.shooter,
                });
            }

            Destroy(bullet.body.gameObject);
            bulletEntity.RemoveComponent<Bullet>();
            World.RemoveEntity(bulletEntity);
        }

        public static BulletHitSystem Create() {
            return CreateInstance<BulletHitSystem>();
        }
    }
}