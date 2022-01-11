namespace Tanks.Movement {
    using Morpeh;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TankMovementInitSystem))]
    public sealed class TankMovementInitSystem : UpdateSystem {
        private Filter tanksNoMove;

        public override void OnAwake() {
            tanksNoMove = World.Filter.With<Tank>().Without<MoveDirection>();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity ent in tanksNoMove) {
                ent.AddComponent<MoveDirection>();
            }
        }

        public static TankMovementInitSystem Create() {
            return CreateInstance<TankMovementInitSystem>();
        }
    }
}