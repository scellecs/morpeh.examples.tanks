namespace Tanks {
    using System;
    using Scellecs.Morpeh;

    [Serializable]
    public struct UserWithTank : IComponent {
        public Entity tank;
    }
}