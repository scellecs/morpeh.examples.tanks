namespace Tanks {
    using UnityEngine;

    [CreateAssetMenu(fileName = "TankConfig", menuName = "Tanks/TankConfiguration", order = 0)]
    public sealed class TankConfig : ScriptableObject {
        public float speed;
    }
}