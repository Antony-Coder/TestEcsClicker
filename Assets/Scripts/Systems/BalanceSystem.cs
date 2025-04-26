using Leopotam.EcsLite;
using UnityEngine;

namespace TestClickerEcs
{
    public class BalanceSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld world;


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

                var entity = world.NewEntity();
                ref var eventBalanceChanged = ref world.GetPool<BalanceChangedEvent>().Add(entity);
            }
        }





        public void Run(IEcsSystems systems)
        {
            foreach(var entity in world.Filter<IncreaseBalanceEvent>().End())
            {
                ref var increaseBalanceEvent = ref world.GetPool<IncreaseBalanceEvent>().Get(entity);

                ChangeBalance(increaseBalanceEvent.Value);
            }


            foreach(var entity in world.Filter<TryBuyEvent>().End())
            {
                ref var request = ref world.GetPool<TryBuyEvent>().Get(entity);

                bool canBuy = GetBalance() >= request.Price;
             

                ref var result = ref world.GetPool<BuyResultEvent>().Add(entity);
                result.Success = canBuy;


                if (canBuy)
                {
                    ChangeBalance(-request.Price);
                }
            }
        }
    }
}
