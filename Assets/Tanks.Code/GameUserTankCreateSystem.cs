namespace Tanks {
    using GameInput;
    using Morpeh;
    using Teams;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GameUserTankCreateSystem))]
    public sealed class GameUserTankCreateSystem : UpdateSystem {
        public TankRepository tankRepository;
        private Filter filter;

        public override void OnAwake() {
            filter = World.Filter.With<GameUser>().Without<UserWithTank>();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity ent in filter) {
                int tankIndex = Random.Range(0, tankRepository.prefabs.Length);
                EntityProvider tankPrefab = tankRepository.prefabs[tankIndex];

                EntityProvider provider = Instantiate(tankPrefab);
                Entity tankEntity = provider.Entity;
                ent.AddComponent<UserWithTank>().tank = tankEntity;
                tankEntity.AddComponent<ControlledByUser>().user = ent;

                Entity teamEntity = ent.GetComponent<InTeam>(out bool isInTeam).team;
                if (!isInTeam) {
                    continue;
                }

                ref Team team = ref teamEntity.GetComponent<Team>();
                Transform tankTransform = provider.transform;
                int spawnIndex = Random.Range(0, team.spawns.Length);
                Transform spawn = team.spawns[spawnIndex];
                tankTransform.position = spawn.position;
                tankTransform.rotation = spawn.rotation;

                tankEntity.AddComponent<InTeam>().team = teamEntity;
            }
        }

        public static GameUserTankCreateSystem Create() {
            return CreateInstance<GameUserTankCreateSystem>();
        }
    }
}