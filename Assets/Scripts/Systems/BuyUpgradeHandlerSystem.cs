using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    public class BuyUpgradeHandlerSystem : IEcsInitSystem, IEcsRunSystem
    {

        private EcsWorld world;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in world.Filter<UpgradeEvent>().End())
            {
                ref var request = ref world.GetPool<BuyResultEvent>().Get(entity);

                if (request.Success)
                {
                    ref var upgradePurchaseComponent = ref world.GetPool<UpgradeEvent>().Get(entity);
                    Upgrade(upgradePurchaseComponent.BusinessEntity, upgradePurchaseComponent.UpgradeId);
                }

            }

        }



        private void Upgrade(int businessId, int upgradeId)
        {
            ref var business = ref world.GetPool<BusinessComponent>().Get(businessId);

            business.UpgradeMultiplier[upgradeId] = business.Settings.Upgrades[upgradeId].ParamsUpgrade.Procent / 100f;

            float multiplier = 0;
            foreach (var m in business.UpgradeMultiplier)
            {
                multiplier += m;
            }
            business.Profit = (int)(business.Level * business.Settings.Params.Profit * (1 + multiplier));
        }

    }
}

