namespace Tanks {
    using Morpeh;
    using UnityEngine;

    [CreateAssetMenu(fileName = "TankRepository", menuName = "Tanks/TankRepository", order = 0)]
    public class TankRepository : ScriptableObject {
        public EntityProvider[] prefabs;
    }
}