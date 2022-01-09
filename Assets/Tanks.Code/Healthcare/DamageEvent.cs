namespace Tanks.Healthcare {
    using System;
    using Morpeh;
    using UnityEngine;

    [Serializable]
    public struct DamageEvent : IComponent {
        public Vector3? hitPosition;
        public float amount;
    }
}