namespace Tanks {
    using System;
    using Morpeh;

    [Serializable]
    public struct UserWithTank : IComponent {
        public Entity tank;
    }
}