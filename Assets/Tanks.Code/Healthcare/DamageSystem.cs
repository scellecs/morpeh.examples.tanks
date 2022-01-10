namespace Tanks.Healthcare {
    using Morpeh;
    using Morpeh.Helpers;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DamageSystem))]
    public sealed class DamageSystem : SimpleUpdateSystem<Health, DamageEvent> {
        protected override void Process(Entity ent, ref Health health, ref DamageEvent damage, in float dt) {
            if (damage.amount <= 0) {
                return;
            }

            health.health -= damage.amount;
            if (health.health > 0) {
                return;
            }

            ent.SetComponent(new IsDeadMarker());
        }

        public static DamageSystem Create() {
            return CreateInstance<DamageSystem>();
        }
    }
}