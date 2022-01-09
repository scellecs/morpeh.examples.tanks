namespace Tanks.Collisions {
    using System;
    using Morpeh;

    [Serializable]
    public struct CollisionEvent : IComponent {
        public Entity first;
        public Entity second;
    }
}