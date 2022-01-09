namespace Tanks {
    using Morpeh;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PhysicsUpdateSystem))]
    public sealed class PhysicsUpdateSystem : FixedUpdateSystem {
        public override void OnAwake() { }

        public override void OnUpdate(float deltaTime) {
            Simulate(deltaTime);
        }

        public static void Simulate(float dt) {
            Physics2D.Simulate(dt);
        }
    }
}