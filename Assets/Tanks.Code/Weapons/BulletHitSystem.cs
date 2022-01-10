namespace Tanks.Weapons {
    using Collisions;
    using Healthcare;
    using Morpeh;
    using Morpeh.Helpers;
    using Teams;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BulletHitSystem))]
    public sealed class BulletHitSystem : SimpleFixedUpdateSystem<CollisionEvent> {
        protected override void Process(Entity ent, ref CollisionEvent evt, in float dt) {
            if (!evt.first.Has<Bullet>()) {
                return;
            }

            Entity bulletEntity = evt.first;
            ref Bullet bullet = ref bulletEntity.GetComponent<Bullet>();
            if (evt.second != null && !evt.second.InSameTeam(bullet.shooter)) {
                evt.second.SetComponent(new DamageEvent {
                        hitPosition = evt.collision?.GetContact(0).point,
                        amount = bullet.config.damage,
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