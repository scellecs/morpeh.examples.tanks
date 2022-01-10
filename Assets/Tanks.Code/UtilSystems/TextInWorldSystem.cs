namespace Tanks.UtilSystems {
    using System;
    using System.Collections.Generic;
    using Morpeh;
    using Morpeh.Helpers;
    using Sirenix.OdinInspector;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TextInWorldSystem))]
    public sealed class TextInWorldSystem : LateUpdateSystem {
        private readonly Stack<TextMesh> textMeshes = new Stack<TextMesh>(8);
        private Filter requests;
        private Filter texts;

        public override void OnAwake() {
            texts = World.Filter.With<TextInWorld>();
            requests = World.Filter.With<Request>();
        }

        public override void OnUpdate(float deltaTime) {
            ProcessTexts(deltaTime);
            ProcessRequests();
        }

        private void ProcessTexts(in float deltaTime) {
            foreach (Entity entity in texts) {
                ref TextInWorld text = ref entity.GetComponent<TextInWorld>();
                text.timeToDestroy -= deltaTime;

                if (text.timeToDestroy > 0) {
                    text.mesh.transform.position += deltaTime * text.velocity;
                    continue;
                }

                text.renderer.forceRenderingOff = true;
                text.mesh.font = null;
                textMeshes.Push(text.mesh);
                entity.RemoveComponent<TextInWorld>();
            }
        }

        private void ProcessRequests() {
            Filter.ComponentsBag<Request> bag = requests.Select<Request>();
            for (int i = 0, length = requests.Length; i < length; i++) {
                SpawnTextInWorld(bag.GetComponent(i));
            }

            requests.RemoveComponentForAll<Request>();
        }

        private void SpawnTextInWorld(in Request request) {
            TextMesh textMesh;
            Renderer renderer;

            if (textMeshes.Count > 0) {
                textMesh = textMeshes.Pop();
                renderer = textMesh.GetComponent<Renderer>();
                renderer.forceRenderingOff = false;
            } else {
                var gameObject = new GameObject("TextInWorld");
                renderer = gameObject.AddComponent<MeshRenderer>();
                textMesh = gameObject.AddComponent<TextMesh>();
            }

            renderer.sortingOrder = 1000;
            textMesh.transform.position = request.start;
            textMesh.alignment = TextAlignment.Center;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.characterSize = request.charSize;
            textMesh.fontSize = request.fontSize;
            textMesh.font = request.font;
            textMesh.color = request.color;
            textMesh.text = request.text;

            World.CreateEntity().SetComponent(new TextInWorld {
                    mesh = textMesh,
                    renderer = renderer,
                    timeToDestroy = request.duration,
                    velocity = request.velocity,
            });
        }

        public static TextInWorldSystem Create() {
            return CreateInstance<TextInWorldSystem>();
        }

        public override void Dispose() {
            foreach (TextMesh textMesh in textMeshes) {
                if (textMesh != null) {
                    Destroy(textMesh.gameObject);
                }
            }

            textMeshes.Clear();
        }

        [Serializable]
        public struct Request : IComponent {
            [Required] public Font font;
            public int fontSize;
            public float charSize;
            public Color color;

            [Space] public Vector3 start;
            public string text;
            public Vector3 velocity;
            [Min(0.5f)] public float duration;
        }

        private struct TextInWorld : IComponent {
            public TextMesh mesh;
            public Renderer renderer;
            public float timeToDestroy;
            public Vector3 velocity;
        }
    }
}