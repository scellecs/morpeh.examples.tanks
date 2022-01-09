namespace Tanks.Teams {
    using System;
    using Morpeh;

    [Serializable]
    public struct InTeam : IComponent {
        public Entity team;
    }

    public static class TeamExtensions {
        public static bool InSameTeam(this Entity first, Entity second) {
            if (!first.Has<InTeam>() || !second.Has<InTeam>()) {
                return false;
            }

            ref InTeam firstInTeam = ref first.GetComponent<InTeam>();
            ref InTeam secondInTeam = ref second.GetComponent<InTeam>();
            return firstInTeam.team.Equals(secondInTeam.team);
        }
    }
}