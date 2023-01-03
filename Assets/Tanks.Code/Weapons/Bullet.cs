namespace Tanks.Weapons {
    using System;
    using Scellecs.Morpeh;
    using UnityEngine;

    [Serializable]
    public struct Bullet : IComponent {
        public BulletConfig config;
        public Rigidbody2D body;
        public Entity shooter;
    }
}