namespace Tanks.Weapons {
    using GameInput;
    using Morpeh;
    using Morpeh.Helpers;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(UserBulletWeaponSystem))]
    public sealed class UserBulletWeaponSystem : SimpleUpdateSystem<BulletWeapon, ControlledByUser> {
        protected override void Process(Entity ent, ref BulletWeapon weapon, ref ControlledByUser controlledByUser, float dt) {
            InputActions inputActions = controlledByUser.user.GetComponent<GameUser>().inputActions;
            weapon.shoot = inputActions.Tank.Fire.phase == InputActionPhase.Started;
        }

        public static UserBulletWeaponSystem Create() {
            return CreateInstance<UserBulletWeaponSystem>();
        }
    }
}