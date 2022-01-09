namespace Tanks.Weapons {
    using System;
    using Morpeh;
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