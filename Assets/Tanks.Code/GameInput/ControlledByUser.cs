namespace Tanks.GameInput {
    using System;
    using Scellecs.Morpeh;

    [Serializable]
    public struct ControlledByUser : IComponent {
        public Entity user;
    }
}