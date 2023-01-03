namespace Tanks.Teams {
    using System;
    using Scellecs.Morpeh;

    [Serializable]
    public struct InTeam : IComponent {
        public Entity team;
    }

    public static class TeamExtensions {
        public static bool InSameTeam(this Entity first, Entity second) {
            ref InTeam firstInTeam = ref first.GetComponent<InTeam>(out bool firstHasTeam);
            ref InTeam secondInTeam = ref second.GetComponent<InTeam>(out bool secondHasTeam);
            if (!firstHasTeam || !secondHasTeam) {
                return false;
            }

            return firstInTeam.team.Equals(secondInTeam.team);
        }
    }
}