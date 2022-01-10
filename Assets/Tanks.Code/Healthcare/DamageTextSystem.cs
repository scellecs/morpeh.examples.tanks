namespace Tanks.Healthcare {
    using Morpeh;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;
    using UtilSystems;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DamageTextSystem))]
    public sealed class DamageTextSystem : UpdateSystem {
        private const string FORMAT = "0.#";

        public TextInWorldSystem.Request textRequest;

        private Filter damageEvents;

        public override void OnAwake() {
            damageEvents = World.Filter.With<DamageEvent>().With<Health>();
        }

        public override void OnUpdate(float deltaTime) {
            CreateNewTexts();
        }

        private void CreateNewTexts() {
            foreach (Entity entity in damageEvents) {
                ref DamageEvent damage = ref entity.GetComponent<DamageEvent>();
                if (!damage.hitPosition.HasValue) {
                    continue;
                }

                string text;
                if (entity.Has<IsDead>()) {
                    text = "IsDead";
                } else {
                    ref Health health = ref entity.GetComponent<Health>();
                    text = $"{health.health.ToString(FORMAT)}HP (-{damage.amount.ToString(FORMAT)})";
                }

                SpawnTextInWorld(damage.hitPosition.Value, text);
            }
        }

        private void SpawnTextInWorld(in Vector3 hitPosition, string text) {
            TextInWorldSystem.Request request = textRequest;
            request.start = hitPosition;
            request.text = text;

            World.CreateEntity().SetComponent(request);
        }

        public static DamageTextSystem Create() {
            return CreateInstance<DamageTextSystem>();
        }
    }
}