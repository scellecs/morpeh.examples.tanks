namespace Tanks.Movement {
    using GameInput;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Helpers;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(UserMovementSystem))]
    public sealed class UserMovementSystem : SimpleUpdateSystem<MoveDirection, ControlledByUser> {
        protected override void Process(Entity ent,
                                        ref MoveDirection moveDirection,
                                        ref ControlledByUser controlledByUser,
                                        in float dt) {
            InputActions inputActions = controlledByUser.user.GetComponent<GameUser>().inputActions;
            moveDirection.direction = inputActions.Tank.Movement.ReadValue<Vector2>();
        }

        public static UserMovementSystem Create() {
            return CreateInstance<UserMovementSystem>();
        }
    }
}