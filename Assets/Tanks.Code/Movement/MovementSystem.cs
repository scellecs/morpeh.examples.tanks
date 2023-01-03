namespace Tanks.Movement {
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Helpers;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MovementSystem))]
    public sealed class MovementSystem : SimpleFixedUpdateSystem<Tank, MoveDirection> {
        protected override void Process(Entity ent, ref Tank tank, ref MoveDirection moveDirection, in float dt) {
            Vector2 direction = moveDirection.direction;
            Vector2 velocity = tank.config.speed * direction;
            tank.body.velocity = velocity;
            tank.body.angularVelocity = 0f;

            if (direction.sqrMagnitude <= 0f) {
                return;
            }

            float angle = Vector2.SignedAngle(Vector2.up, direction);
            tank.body.rotation = angle;
        }

        public static MovementSystem Create() {
            return CreateInstance<MovementSystem>();
        }
    }
}