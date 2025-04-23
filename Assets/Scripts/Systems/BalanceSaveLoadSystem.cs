using Leopotam.EcsLite;
using UnityEngine;

namespace TestClickerEcs
{
    public class BalanceSaveLoadSystem : IEcsInitSystem, IEcsDestroySystem
    {
        private SaveLoadService saveLoadService;
        private EcsWorld world;

        private const string saveKey = "BalanceSave";

        public BalanceSaveLoadSystem(SaveLoadService saveLoadService)
        {
            this.saveLoadService = saveLoadService;
            saveLoadService.SaveEvent += Save;
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



        public void Destroy(IEcsSystems systems)
        {
            Save();
        }

        public void Save()
        {
            if (world == null) return;

            Debug.Log("SAVE");

            BalanceSave save = new BalanceSave();

            foreach (var entity in world.Filter<BalanceComponent>().End())
            {
                ref var balanceComponent = ref world.GetPool<BalanceComponent>().Get(entity);
                save.Dollars = balanceComponent.Value;
            }

            saveLoadService.Save<BalanceSave>(saveKey, save);
        }
    }
}
