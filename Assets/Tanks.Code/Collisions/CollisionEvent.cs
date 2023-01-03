namespace Tanks.Collisions {
    using System;
    using Scellecs.Morpeh;
    using UnityEngine;

    [Serializable]
    public struct CollisionEvent : IComponent {
        public Collision2D collision;
        public Entity first;
        public Entity second;
    }
}