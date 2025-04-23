using Leopotam.EcsLite;
using UnityEngine;

namespace TestClickerEcs
{
    public class ProfitSystem : IEcsRunSystem
    {

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            foreach (var entity in world.Filter<BusinessComponent>().End())
            {
                ref var business = ref world.GetPool<BusinessComponent>().Get(entity);

                if (business.Profit <= 0) continue;

                business.Timer += Time.deltaTime;
                business.ProfitProgress = business.Timer / business.Settings.Params.ProfitDelay;

                if (business.Timer >= business.Settings.Params.ProfitDelay)
                {
                    business.Timer = 0;

                    int newEntity = world.NewEntity();
                    ref var increaseBalance = ref world.GetPool<IncreaseBalanceEvent>().Add(newEntity);
                    increaseBalance.Value = business.Profit;
                }

            }
        }

    }
}

