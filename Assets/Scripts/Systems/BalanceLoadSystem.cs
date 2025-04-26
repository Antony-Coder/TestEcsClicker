using Leopotam.EcsLite;
using UnityEngine;

namespace TestClickerEcs
{
    public class BalanceLoadSystem : IEcsInitSystem
    {
        private SaveLoadService saveLoadService;
        private EcsWorld world;

        private const string saveKey = "BalanceSave";

        public BalanceLoadSystem(SaveLoadService saveLoadService)
        {
            this.saveLoadService = saveLoadService;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();

            SharedData sharedData = systems.GetShared<SharedData>();

            BalanceSave save = saveLoadService.Load<BalanceSave>(saveKey, new BalanceSave());

            int entity = world.NewEntity();
            ref var balanceComponent = ref world.GetPool<BalanceComponent>().Add(entity);

            balanceComponent.View = sharedData.BalanceView;
            balanceComponent.Value = save.Dollars;
        }



    }
}
