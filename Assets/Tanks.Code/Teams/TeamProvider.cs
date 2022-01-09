namespace Tanks.Teams {
    using System;
    using Morpeh;
    using UnityEngine;

    [AddComponentMenu("Tanks/Team")]
    public sealed class TeamProvider : MonoProvider<Team> { }

    [Serializable]
    public struct Team : IComponent {
        public string name;
        public Color color;
        public int userCount;
        public Transform[] spawns;
    }
}