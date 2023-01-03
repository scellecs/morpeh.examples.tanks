namespace Tanks.Scores {
    using System;
    using Scellecs.Morpeh;

    [Serializable]
    public struct UserScores : IComponent {
        public int totalKills;
    }
}