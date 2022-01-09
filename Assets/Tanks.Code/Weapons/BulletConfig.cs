namespace Tanks.Weapons {
    using UnityEngine;

    [CreateAssetMenu(fileName = "BulletConfig", menuName = "Tanks/BulletConfig", order = 0)]
    public sealed class BulletConfig : ScriptableObject {
        public Rigidbody2D prefab;
        public float damage;
    }
}