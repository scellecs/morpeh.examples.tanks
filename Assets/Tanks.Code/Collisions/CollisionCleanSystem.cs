namespace Tanks.Collisions {
    using Morpeh;
    using Morpeh.Helpers;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CollisionCleanSystem))]
    public sealed class CollisionCleanSystem : SimpleLateUpdateSystem<CollisionEvent> {
        protected override void Process(Entity ent, ref CollisionEvent evt, in float dt) {
            World.RemoveEntity(ent);
        }

        public static CollisionCleanSystem Create() {
            return CreateInstance<CollisionCleanSystem>();
        }
    }
}