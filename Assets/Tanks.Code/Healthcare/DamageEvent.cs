namespace Tanks.Healthcare {
    using System;
    using Morpeh;

    [Serializable]
    public struct DamageEvent : IComponent {
        public float amount;
    }
}