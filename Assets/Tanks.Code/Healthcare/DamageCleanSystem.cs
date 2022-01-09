namespace Tanks.Healthcare {
    using Morpeh;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DamageCleanSystem))]
    public sealed class DamageCleanSystem : LateUpdateSystem {
        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<DamageEvent>();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity ent in filter) {
                ent.RemoveComponent<DamageEvent>();
            }
        }

        public static DamageCleanSystem Create() {
            return CreateInstance<DamageCleanSystem>();
        }
    }
}