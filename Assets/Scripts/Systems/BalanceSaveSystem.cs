using Leopotam.EcsLite;
using UnityEngine;

namespace TestClickerEcs
{
    public class BalanceSaveSystem :  IEcsRunSystem
    {
        private SaveLoadService saveLoadService;

        private const string saveKey = "BalanceSave";

        public BalanceSaveSystem(SaveLoadService saveLoadService)
        {
            this.saveLoadService = saveLoadService;
        }


        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            foreach (var entity in world.Filter<BalanceComponent>().End())
            {
                ref var balanceComponent = ref world.GetPool<BalanceComponent>().Get(entity);
                BalanceSave save = new BalanceSave();
                save.Dollars = balanceComponent.Value;
                saveLoadService.Save<BalanceSave>(saveKey, save);

            }

        }



    }
}
