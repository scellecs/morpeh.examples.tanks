namespace Tanks.Healthcare {
    using System;
    using JetBrains.Annotations;
    using Scellecs.Morpeh;
    using UnityEngine;

    [Serializable]
    public struct DamageEvent : IComponent {
        public Vector3? hitPosition;
        public float amount;
        [CanBeNull] public Entity dealer;
    }
}