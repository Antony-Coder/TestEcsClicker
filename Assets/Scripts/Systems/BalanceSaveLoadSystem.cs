using Leopotam.EcsLite;
using UnityEngine;

namespace TestClickerEcs
{
    public class BalanceSaveLoadSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private SaveLoadService saveLoadService;

        private const string saveKey = "BalanceSave";

        public BalanceSaveLoadSystem(SaveLoadService saveLoadService)
        {
            this.saveLoadService = saveLoadService;
        }

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            SharedData sharedData = systems.GetShared<SharedData>();

            BalanceSave save = saveLoadService.Load<BalanceSave>(saveKey, new BalanceSave());

            int entity = world.NewEntity();
            ref var balanceComponent = ref world.GetPool<BalanceComponent>().Add(entity);

            balanceComponent.View = sharedData.BalanceView;
            balanceComponent.Value = save.Dollars;
        }





        public void Destroy(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            BalanceSave save = new BalanceSave();

            foreach (var entity in world.Filter<BalanceComponent>().End())
            {
                ref var balanceComponent = ref world.GetPool<BalanceComponent>().Get(entity);
                save.Dollars = balanceComponent.Value;
            }

            saveLoadService.Save<BalanceSave>(saveKey,save);
        }
    }
}
