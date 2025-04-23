using Leopotam.EcsLite;
using UnityEngine;

namespace TestClickerEcs
{
    public class BalanceUpdateViewSystem : IEcsInitSystem,  IEcsEventListener<BalanceChangedEvent>
    {
        private EcsWorld world;
        private SharedData sharedData;


        public BalanceUpdateViewSystem(EventService eventService)
        {
            eventService.AddListener<BalanceChangedEvent>(this);
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
            sharedData = systems.GetShared<SharedData>();
            UpdateView();
        }

        public void OnEvent(ref BalanceChangedEvent evt)
        {
            UpdateView();
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
