namespace Tanks.Walls {
    using System;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Providers;
    using UnityEngine;

    [AddComponentMenu("Tanks/Wall")]
    public sealed class WallProvider : MonoProvider<Wall> { }

    [Serializable]
    public struct Wall : IComponent {
        public Transform transform;
    }
}