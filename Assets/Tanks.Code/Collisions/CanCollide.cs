namespace Tanks.Collisions {
    using System;
    using Scellecs.Morpeh;

    [Serializable]
    public struct CanCollide : IComponent {
        public CollisionDetector detector;
    }
}