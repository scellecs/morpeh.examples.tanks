namespace Tanks.Walls {
    using System;
    using Morpeh;
    using UnityEngine;

    [AddComponentMenu("Tanks/Wall")]
    public sealed class WallProvider : MonoProvider<Wall> { }

    [Serializable]
    public struct Wall : IComponent {
        public Transform transform;
    }
}