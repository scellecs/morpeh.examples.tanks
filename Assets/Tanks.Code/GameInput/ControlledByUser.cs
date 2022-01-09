namespace Tanks.GameInput {
    using System;
    using Morpeh;

    [Serializable]
    public struct ControlledByUser : IComponent {
        public Entity user;
    }
}