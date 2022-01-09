namespace Tanks.Healthcare {
    using GameInput;
    using Morpeh;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TankDestroySystem))]
    public sealed class TankDestroySystem : UpdateSystem {
        private Filter destroyedTanks;

        public override void OnAwake() {
            destroyedTanks = World.Filter.With<Tank>().With<IsDead>();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity ent in destroyedTanks) {
                if (ent.Has<ControlledByUser>()) {
                    Entity userEntity = ent.GetComponent<ControlledByUser>().user;
                    userEntity.RemoveComponent<UserWithTank>();
                }

                GameObject tankGo = ent.GetComponent<Tank>().body.gameObject;
                World.RemoveEntity(ent);
                Destroy(tankGo);
            }
        }

        public static TankDestroySystem Create() {
            return CreateInstance<TankDestroySystem>();
        }
    }
}