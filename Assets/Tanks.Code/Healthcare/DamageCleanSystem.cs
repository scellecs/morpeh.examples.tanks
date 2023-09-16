namespace Tanks.Healthcare {
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DamageCleanSystem))]
    public sealed class DamageCleanSystem : LateUpdateSystem {
        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<DamageEvent>().Build();
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