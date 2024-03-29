﻿namespace Tanks.Walls {
    using Healthcare;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WallDestroySystem))]
    public sealed class WallDestroySystem : UpdateSystem {
        private Filter destroyedWalls;

        public override void OnAwake() {
            destroyedWalls = World.Filter.With<Wall>().With<IsDeadMarker>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity ent in destroyedWalls) {
                GameObject wallGo = ent.GetComponent<Wall>().transform.gameObject;
                World.RemoveEntity(ent);
                Destroy(wallGo);
            }
        }

        public static WallDestroySystem Create() {
            return CreateInstance<WallDestroySystem>();
        }
    }
}