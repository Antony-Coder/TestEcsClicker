using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    public class BuyLevelSystem : IEcsInitSystem
    {
        private EcsWorld world;

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();

            foreach (var entity in world.Filter<BusinessComponent>().End())
            {
                ref var business = ref world.GetPool<BusinessComponent>().Get(entity);
                int businessEntity = entity;
                business.View.LevelUp.Button.onClick.AddListener(()=>TryBuy(businessEntity));
            }
        }

        private void TryBuy(int businessEntity)
        {
            ref var business = ref world.GetPool<BusinessComponent>().Get(businessEntity);

            int entity = world.NewEntity();
            ref var tryBuyEvent = ref world.GetPool<TryBuyEvent>().Add(entity);
            tryBuyEvent.Price = business.LevelUpPrice;

            ref var levelPurchaseComponent = ref world.GetPool<LevelUpEvent>().Add(entity);
            levelPurchaseComponent.BusinessEntity = businessEntity;
        }





    }
}
