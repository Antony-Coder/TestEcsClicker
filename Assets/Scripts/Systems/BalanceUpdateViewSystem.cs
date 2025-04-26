using Leopotam.EcsLite;
using UnityEngine;

namespace TestClickerEcs
{
    public class BalanceUpdateViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;
        private SharedData sharedData;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            sharedData = systems.GetShared<SharedData>();
            UpdateView();
        }



        public void Run(IEcsSystems systems)
        {
            foreach(var entity in world.Filter<BalanceChangedEvent>().End())
            {
                UpdateView();
            }
        }

        private void UpdateView()
        {
            foreach (var entity in world.Filter<BalanceComponent>().End())
            {
                ref var balanceComponent = ref world.GetPool<BalanceComponent>().Get(entity);
                balanceComponent.View.Value.text = string.Format(sharedData.BusinessWindowConfig.Balance, balanceComponent.Value);
            }
        }
    }
}
