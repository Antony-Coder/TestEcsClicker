using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    public class BuyLevelHandlerSystem : IEcsInitSystem, IEcsRunSystem
    {

        private EcsWorld world;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {

            foreach (var entity in world.Filter<LevelUpEvent>().End())
            {
                ref var request = ref world.GetPool<BuyResultEvent>().Get(entity);

                if (request.Success)
                {
                    ref var levelPurchaseComponent = ref world.GetPool<LevelUpEvent>().Get(entity);
                    LevelUp(levelPurchaseComponent.BusinessEntity);
                }


            }
        }

        private void LevelUp(int businessEntity)
        {
            ref var business = ref world.GetPool<BusinessComponent>().Get(businessEntity);

            business.Level++;

            business.LevelUpPrice = (business.Level + 1) * business.Settings.Params.Price;

            float multiplier = 0;
            foreach (var m in business.UpgradeMultiplier)
            {
                multiplier += m;
            }
            business.Profit = (int)(business.Level * business.Settings.Params.Profit * (1 + multiplier));
        }
    }
}
