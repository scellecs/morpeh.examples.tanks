namespace Tanks.Healthcare {
    using GameInput;
    using Morpeh;
    using Morpeh.Helpers;
    using Scores;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TankDestroySystem))]
    public sealed class TankDestroySystem : UpdateSystem {
        private Filter destroyedTanks;

        public override void OnAwake() {
            destroyedTanks = World.Filter.With<Tank>().With<IsDeadMarker>();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity ent in destroyedTanks) {
                IncreaseStatForKiller(ent);

                ref ControlledByUser controlledByUser = ref ent.GetComponent<ControlledByUser>(out bool isControlled);
                if (isControlled) {
                    controlledByUser.user.RemoveComponent<UserWithTank>();
                }

                GameObject tankGo = ent.GetComponent<Tank>().body.gameObject;
                World.RemoveEntity(ent);
                Destroy(tankGo);
            }
        }

        private static void IncreaseStatForKiller(Entity ent) {
            ref DamageEvent damageEvent = ref ent.GetComponent<DamageEvent>(out bool isDamaged);
            if (isDamaged) {
                damageEvent.dealer?.GetOrCreate<OneMoreKillEvent>();
            }
        }

        public static TankDestroySystem Create() {
            return CreateInstance<TankDestroySystem>();
        }
    }
}