namespace Tanks.Scores {
    using System;
    using Morpeh;

    [Serializable]
    public struct UserScores : IComponent {
        public int totalKills;
    }
}