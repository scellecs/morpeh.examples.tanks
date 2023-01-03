namespace Tanks {
    using Scellecs.Morpeh.Providers;
    using UnityEngine;

    [CreateAssetMenu(fileName = "TankRepository", menuName = "Tanks/TankRepository", order = 0)]
    public sealed class TankRepository : ScriptableObject {
        public EntityProvider[] prefabs;
    }
}