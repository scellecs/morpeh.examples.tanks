namespace Tanks.Collisions {
    using System;
    using Morpeh;

    [Serializable]
    public struct CanCollide : IComponent {
        public CollisionDetector detector;
    }
}