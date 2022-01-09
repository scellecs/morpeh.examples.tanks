namespace Tanks.Healthcare {
    using System.Collections.Generic;
    using Morpeh;
    using Sirenix.OdinInspector;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DamageTextSystem))]
    public sealed class DamageTextSystem : UpdateSystem {
        private const string FORMAT = "0.#";

        [Min(0.5f)] public float duration = 3f;
        public Vector3 velocity = Vector3.up;
        public float charSize = 1f;
        public int fontSize = 5;
        [Required] public Font font;

        private readonly Stack<TextMesh> textMeshes = new Stack<TextMesh>(8);

        private Filter damageEvents;
        private Filter texts;

        public override void OnAwake() {
            damageEvents = World.Filter.With<DamageEvent>().With<Health>();
            texts = World.Filter.With<TextInWorld>();
        }

        public override void OnUpdate(float deltaTime) {
            ProcessTexts(deltaTime);
            CreateNewTexts();
        }

        private void ProcessTexts(in float deltaTime) {
            Vector3 currentVelocity = deltaTime * velocity;

            foreach (Entity entity in texts) {
                ref TextInWorld text = ref entity.GetComponent<TextInWorld>();
                text.timeToDestroy -= deltaTime;

                if (text.timeToDestroy > 0) {
                    text.mesh.transform.position += currentVelocity;
                    continue;
                }

                text.renderer.forceRenderingOff = true;
                textMeshes.Push(text.mesh);
                entity.RemoveComponent<TextInWorld>();
            }
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

        private void SpawnTextInWorld(in Vector3 position, string text) {
            TextMesh textMesh;
            Renderer renderer;

            if (textMeshes.Count > 0) {
                textMesh = textMeshes.Pop();
                renderer = textMesh.GetComponent<Renderer>();
                renderer.forceRenderingOff = false;
            } else {
                var gameObject = new GameObject("DamageText");
                renderer = gameObject.AddComponent<MeshRenderer>();
                textMesh = gameObject.AddComponent<TextMesh>();
            }

            renderer.sortingOrder = 1000;
            textMesh.transform.position = position;
            textMesh.alignment = TextAlignment.Center;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.characterSize = charSize;
            textMesh.fontSize = fontSize;
            textMesh.font = font;
            textMesh.text = text;

            World.CreateEntity().SetComponent(new TextInWorld {
                    mesh = textMesh,
                    renderer = renderer,
                    timeToDestroy = duration,
            });
        }

        public override void Dispose() {
            foreach (TextMesh textMesh in textMeshes) {
                if (textMesh != null) {
                    Destroy(textMesh.gameObject);
                }
            }

            textMeshes.Clear();
        }

        public static DamageTextSystem Create() {
            return CreateInstance<DamageTextSystem>();
        }

        private struct TextInWorld : IComponent {
            public TextMesh mesh;
            public Renderer renderer;
            public float timeToDestroy;
        }
    }
}