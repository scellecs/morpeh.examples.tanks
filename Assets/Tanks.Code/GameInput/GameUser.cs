namespace Tanks.GameInput {
    using System;
    using Scellecs.Morpeh;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.Users;

    [Serializable]
    public struct GameUser : IComponent, IDisposable {
        public InputDevice device;
        public InputActions inputActions;
        public InputUser user;

        public void Dispose() {
            inputActions?.Disable();

            if (!user.valid) {
                return;
            }

            user.UnpairDevicesAndRemoveUser();
        }
    }
}