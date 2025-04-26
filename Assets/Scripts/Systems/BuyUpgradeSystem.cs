using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    public class BuyUpgradeSystem : IEcsInitSystem
    {
        private EcsWorld world;


        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();

            foreach (var entity in world.Filter<BusinessComponent>().End())
            {
                ref var business = ref world.GetPool<BusinessComponent>().Get(entity);

                int businessEntity = entity;
                

                for (int i = 0; i < business.View.Upgrades.Length; i++)
                {
                    int upgradeId = i;
                    business.View.Upgrades[i].Button.onClick.AddListener(() => TryBuy(businessEntity, upgradeId));
                }

            }
        }

        private void TryBuy(int businessEntity, int upgradeId)
        {
            ref var business = ref world.GetPool<BusinessComponent>().Get(businessEntity);

            int entity = world.NewEntity();
            ref var tryBuyEvent = ref world.GetPool<TryBuyEvent>().Add(entity);
            tryBuyEvent.Price = business.LevelUpPrice;

            ref var upgradePurchaseComponent = ref world.GetPool<UpgradeEvent>().Add(entity);

            upgradePurchaseComponent.BusinessEntity = businessEntity;
            upgradePurchaseComponent.UpgradeId = upgradeId;
        }



    }
}
