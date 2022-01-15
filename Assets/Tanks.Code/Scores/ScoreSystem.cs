namespace Tanks.Scores {
    using GameInput;
    using Morpeh;
    using Morpeh.Helpers;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;
    using UtilSystems;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ScoreSystem))]
    public sealed class ScoreSystem : LateUpdateSystem {
        public TextInWorldSystem.Request killMessage;

        private Filter usersToInit;
        private Filter killEvents;

        public override void OnAwake() {
            usersToInit = World.Filter.With<GameUser>().Without<UserScores>();
            killEvents = World.Filter.With<OneMoreKillEvent>().With<ControlledByUser>();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity entity in usersToInit) {
                entity.AddComponent<UserScores>();
            }

            ScoreKills();
        }

        private void ScoreKills() {
            World.UpdateFilters();

            foreach (Entity entity in killEvents) {
                Entity userEntity = entity.GetComponent<ControlledByUser>().user;
                ref UserScores scores = ref userEntity.GetComponent<UserScores>();
                scores.totalKills++;

                ref Tank tank = ref entity.GetComponent<Tank>(out bool isTank);
                if (!isTank) {
                    Debug.LogError("Able to show kill messages only for Tanks!");
                    continue;
                }

                TextInWorldSystem.Request request = killMessage;
                request.start = tank.body.position;
                request.text = $"{scores.totalKills.ToString()} kills";
                World.CreateEntity().SetComponent(request);
            }

            killEvents.RemoveComponentForAll<OneMoreKillEvent>();
        }

        public static ScoreSystem Create() {
            return CreateInstance<ScoreSystem>();
        }
    }
}