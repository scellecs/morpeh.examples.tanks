namespace Tanks.GameInput {
    using System;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.Controls;
    using UnityEngine.InputSystem.LowLevel;
    using UnityEngine.InputSystem.Users;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GameInputSystem))]
    public sealed class GameInputSystem : UpdateSystem {
        private Action<InputControl, InputEventPtr> unpairedDeviceUsedDelegate;

        private int userCounter;
        private Filter users;

        public override void OnAwake() {
            World.GetStash<GameUser>().AsDisposable();
            users = World.Filter.With<GameUser>().Build();
            userCounter = 0;

            unpairedDeviceUsedDelegate = OnUnpairedDeviceUsed;
            ++InputUser.listenForUnpairedDeviceActivity;
            InputUser.onUnpairedDeviceUsed += unpairedDeviceUsedDelegate;
        }

        public override void OnUpdate(float deltaTime) {
            InputSystem.Update();
        }

        public override void Dispose() {
            InputUser.onUnpairedDeviceUsed -= unpairedDeviceUsedDelegate;
            --InputUser.listenForUnpairedDeviceActivity;

            foreach (Entity ent in users) {
                ent.RemoveComponent<GameUser>();
            }
        }

        private void OnUnpairedDeviceUsed(InputControl control, InputEventPtr eventPtr) {
            if (!(control is ButtonControl)) {
                return;
            }

            var actions = new InputActions();
            if (!actions.CommonScheme.SupportsDevice(control.device)) {
                return;
            }

            Entity userEntity = World.CreateEntity();
            ref GameUser user = ref userEntity.AddComponent<GameUser>();
            user.id = ++userCounter;
            user.device = control.device;
            Debug.Log($"{user.device} (Id={user.id.ToString()}) connected!");

            user.inputActions = actions;
            user.inputActions.Enable();

            user.user = InputUser.PerformPairingWithDevice(control.device);
            user.user.ActivateControlScheme(actions.CommonScheme);
            user.user.AssociateActionsWithUser(actions);
        }

        public static GameInputSystem Create() {
            return CreateInstance<GameInputSystem>();
        }
    }
}