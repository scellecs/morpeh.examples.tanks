﻿namespace Tanks.Collisions {
    using System;
    using Scellecs.Morpeh;
    using UnityEngine;

    public sealed class CollisionDetector : MonoBehaviour {
        public Collider2D[] colliders;
        public Entity listener;

        private World world;

        public void Init(World world) {
            this.world = world;
        }

        private void OnEnable() {
            colliders = GetComponentsInChildren<Collider2D>();

#if DEBUG
            if (colliders.Length <= 0) {
                throw new Exception($"There are no any Colliders to handle collisions on {name}");
            }
#endif
        }

        private void OnCollisionEnter2D(Collision2D other) {
#if DEBUG
            if (listener == null || !listener.Has<CanCollide>()) {
                throw new Exception($"{nameof(listener)} should have {nameof(CanCollide)}");
            }
#endif

            Entity evtEntity = world.CreateEntity();
            ref CollisionEvent evt = ref evtEntity.AddComponent<CollisionEvent>();
            evt.collision = other;
            evt.first = listener;

            var otherDetector = other.gameObject.GetComponent<CollisionDetector>();
            evt.second = otherDetector != null ? otherDetector.listener : null;
        }
    }
}