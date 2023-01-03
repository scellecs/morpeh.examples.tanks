namespace Tanks.Weapons {
    using System;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Providers;
    using UnityEngine;

    [AddComponentMenu("Tanks/BulletWeapon")]
    public sealed class BulletWeaponProvider : MonoProvider<BulletWeapon> { }

    [Serializable]
    public struct BulletWeapon : IComponent {
        public BulletWeaponConfig config;
        public bool shoot;
        public float lastShotTime;
    }
}