namespace Tanks {
    using UnityEngine;

    [CreateAssetMenu(fileName = "TankConfig", menuName = "Tanks/TankConfiguration", order = 0)]
    public class TankConfig : ScriptableObject {
        public float speed;
    }
}