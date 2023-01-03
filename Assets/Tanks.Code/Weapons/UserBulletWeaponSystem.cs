namespace Tanks.Weapons {
    using GameInput;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Helpers;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(UserBulletWeaponSystem))]
    public sealed class UserBulletWeaponSystem : SimpleUpdateSystem<BulletWeapon, ControlledByUser> {
        protected override void Process(Entity ent,
                                        ref BulletWeapon weapon,
                                        ref ControlledByUser controlledByUser,
                                        in float dt) {
            InputActionPhase actionPhase = controlledByUser.user.GetComponent<GameUser>().inputActions.Tank.Fire.phase;
            weapon.shoot = actionPhase == InputActionPhase.Started || actionPhase == InputActionPhase.Performed;
        }

        public static UserBulletWeaponSystem Create() {
            return CreateInstance<UserBulletWeaponSystem>();
        }
    }
}