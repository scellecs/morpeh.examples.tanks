namespace Tanks.Bases {
    using System;
    using Morpeh;
    using Teams;
    using UnityEngine;

    [AddComponentMenu("Tanks/TeamBase")]
    public sealed class TeamBaseProvider : MonoProvider<TeamBase> {
        public TeamProvider team;

        protected override void Initialize() {
            base.Initialize();
            Entity.AddComponent<InTeam>().team = team.Entity;
        }
    }

    [Serializable]
    public struct TeamBase : IComponent {
        public TeamBaseView view;
    }
}