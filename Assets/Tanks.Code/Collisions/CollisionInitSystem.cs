namespace Tanks.Collisions {
    using Bases;
    using Morpeh;
    using UnityEngine;
    using Walls;
    using Weapons;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CollisionInitSystem))]
    public sealed class CollisionInitSystem : FixedUpdateSystem {
        private Filter bases;
        private Filter bullets;
        private Filter tanks;
        private Filter walls;

        public override void OnAwake() {
            tanks = World.Filter.With<Tank>().Without<CanCollide>();
            bullets = World.Filter.With<Bullet>().Without<CanCollide>();
            walls = World.Filter.With<Wall>().Without<CanCollide>();
            bases = World.Filter.With<TeamBase>().Without<CanCollide>();
        }

        public override void OnUpdate(float deltaTime) {
            ProcessTanks();
            ProcessBullets();
            ProcessWalls();
            ProcessBases();
        }

        private void ProcessTanks() {
            foreach (Entity ent in tanks) {
                ref Tank tank = ref ent.GetComponent<Tank>();
                MakeCanCollide(ent, tank.body.gameObject);
            }
        }

        private void ProcessBullets() {
            foreach (Entity ent in bullets) {
                ref Bullet bullet = ref ent.GetComponent<Bullet>();
                MakeCanCollide(ent, bullet.body.gameObject);
            }
        }

        private void ProcessWalls() {
            foreach (Entity ent in walls) {
                ref Wall wall = ref ent.GetComponent<Wall>();
                MakeCanCollide(ent, wall.transform.gameObject);
            }
        }

        private void ProcessBases() {
            foreach (Entity ent in bases) {
                ref TeamBase teamBase = ref ent.GetComponent<TeamBase>();
                MakeCanCollide(ent, teamBase.view.gameObject);
            }
        }

        private void MakeCanCollide(Entity entity, GameObject gameObject) {
            ref CanCollide canCollide = ref entity.AddComponent<CanCollide>();
            canCollide.detector = gameObject.AddComponent<CollisionDetector>();
            canCollide.detector.Init(World);
            canCollide.detector.listener = entity;
        }

        public static CollisionInitSystem Create() {
            return CreateInstance<CollisionInitSystem>();
        }
    }
}