namespace Tanks.Movement {
    using GameInput;
    using Morpeh;
    using Morpeh.Helpers;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(UserMovementSystem))]
    public class UserMovementSystem : SimpleUpdateSystem<MoveDirection, ControlledByUser> {
        protected override void Process(Entity ent, ref MoveDirection _, ref ControlledByUser controlledByUser, float dt) {
            InputActions inputActions = controlledByUser.user.GetComponent<GameUser>().inputActions;
            ent.SetComponent(new MoveDirection {
                    direction = inputActions.Tank.Movement.ReadValue<Vector2>(),
            });
        }

        public static UserMovementSystem Create() {
            return CreateInstance<UserMovementSystem>();
        }
    }
}