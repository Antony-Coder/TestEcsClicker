using Leopotam.EcsLite;
using UnityEngine;

namespace TestClickerEcs
{
    public class BalanceSystem : IEcsInitSystem,  IEcsEventListener<IncreaseBalanceEvent>, IEcsEventListener<TryBuyEvent>
    {

        private EcsWorld world;

        public BalanceSystem(EventService eventService)
        {
            eventService.AddListener<IncreaseBalanceEvent>(this);
            eventService.AddListener<TryBuyEvent>(this);
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
        }



        private int GetBalance()
        {
            foreach (var balanceEntity in world.Filter<BalanceComponent>().End())
            {
                ref var balance = ref world.GetPool<BalanceComponent>().Get(balanceEntity);
                return balance.Value;
            }

            return 0;
        }

        private void ChangeBalance(int amount)
        {
            foreach (var balanceEntity in world.Filter<BalanceComponent>().End())
            {
                ref var balance = ref world.GetPool<BalanceComponent>().Get(balanceEntity);
                balance.Value += amount;

                if (amount != 0)
                {
                    var entity = world.NewEntity();
                    ref var eventBalanceChanged = ref world.GetPool<BalanceChangedEvent>().Add(entity);
                }

                
            }
        }

        public void OnEvent(ref IncreaseBalanceEvent evt)
        {
            ChangeBalance(evt.Value);
        }

        public void OnEvent(ref TryBuyEvent request)
        {
            bool canBuy = GetBalance() >= request.Price;

            var resultEntity = world.NewEntity();
            ref var result = ref world.GetPool<BuyResultEvent>().Add(resultEntity);
            result.Success = canBuy;
            result.RequestId = request.RequestId;

            if (canBuy)
            {
                ChangeBalance(-request.Price);
            }
        }
    }
}
