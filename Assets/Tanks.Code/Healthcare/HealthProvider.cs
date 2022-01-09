namespace Tanks.Healthcare {
    using System;
    using Morpeh;
    using UnityEngine;

    [AddComponentMenu("Tanks/Health")]
    public sealed class HealthProvider : MonoProvider<Health> { }

    [Serializable]
    public struct Health : IComponent {
        public float health;
    }
}