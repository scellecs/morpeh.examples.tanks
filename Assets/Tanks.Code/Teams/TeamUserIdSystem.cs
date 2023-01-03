namespace Tanks.Teams {
    using System;
    using GameInput;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TeamUserIdSystem))]
    public sealed class TeamUserIdSystem : LateUpdateSystem {
        public TextMesh userIdTextPrefab;
        private Filter tanksToDisplay;

        public override void OnAwake() {
            World.GetStash<UserIdText>().AsDisposable();
            tanksToDisplay = World.Filter.With<Tank>().With<ControlledByUser>().Without<UserIdText>();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity tankEntity in tanksToDisplay) {
                Entity userEntity = tankEntity.GetComponent<ControlledByUser>().user;
                if (userEntity.IsNullOrDisposed()) {
                    continue;
                }

                ref Tank tank = ref tankEntity.GetComponent<Tank>();
                ref GameUser user = ref userEntity.GetComponent<GameUser>();

                ref UserIdText userIdText = ref tankEntity.AddComponent<UserIdText>();
                userIdText.text = Instantiate(userIdTextPrefab, tank.body.transform);
                userIdText.text.GetComponent<Renderer>().sortingOrder = 10;
                userIdText.text.transform.localPosition = tank.userTextOffset;
                userIdText.text.text = $"#{user.id.ToString()}";
                userIdText.text.color = Color.white;

                ref InTeam inTeam = ref tankEntity.GetComponent<InTeam>(out bool isInTeam);
                if (isInTeam) {
                    userIdText.text.color = inTeam.team.GetComponent<Team>().color;
                }
            }
        }

        public static TeamUserIdSystem Create() => CreateInstance<TeamUserIdSystem>();

        private struct UserIdText : IComponent, IDisposable {
            public TextMesh text;

            public readonly void Dispose() {
                if (text != null) {
                    Destroy(text.gameObject);
                }
            }
        }
    }
}