namespace Tanks {
    using GameInput;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Providers;
    using Scellecs.Morpeh.Systems;
    using Teams;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GameUserTankCreateSystem))]
    public sealed class GameUserTankCreateSystem : UpdateSystem {
        public TankRepository tankRepository;
        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<GameUser>().Without<UserWithTank>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity userEntity in filter) {
                SpawnTankForUser(userEntity, out Entity tankEntity, out Transform tankTransform);

                Entity teamEntity = userEntity.GetComponent<InTeam>(out bool isInTeam).team;
                if (isInTeam) {
                    AttachTankToTeam(tankEntity, tankTransform, teamEntity);
                } else {
                    Debug.LogError("User without team!");
                }
            }
        }

        private void SpawnTankForUser(Entity userEntity, out Entity tankEntity, out Transform tankTransform) {
            int tankIndex = Random.Range(0, tankRepository.prefabs.Length);
            EntityProvider tankPrefab = tankRepository.prefabs[tankIndex];
            EntityProvider tankInstance = Instantiate(tankPrefab);
            tankEntity = tankInstance.Entity;
            tankTransform = tankInstance.transform;
            userEntity.AddComponent<UserWithTank>().tank = tankEntity;
            tankEntity.AddComponent<ControlledByUser>().user = userEntity;
        }

        private static void AttachTankToTeam(Entity tankEntity, Transform tankTransform, Entity teamEntity) {
            ref Team team = ref teamEntity.GetComponent<Team>();
            int spawnIndex = Random.Range(0, team.spawns.Length);
            Transform spawn = team.spawns[spawnIndex];
            tankTransform.position = spawn.position;
            tankTransform.rotation = spawn.rotation;
            tankEntity.AddComponent<InTeam>().team = teamEntity;
        }

        public static GameUserTankCreateSystem Create() {
            return CreateInstance<GameUserTankCreateSystem>();
        }
    }
}