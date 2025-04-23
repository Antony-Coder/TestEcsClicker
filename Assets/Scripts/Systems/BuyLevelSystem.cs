using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    public class BuyLevelSystem : IEcsInitSystem,  IEcsEventListener<BuyResultEvent>
    {
        private EcsWorld world;

        private HashSet<Request> requests = new HashSet<Request>();

        private struct Request
        {
            public int Id;
            public int BusinessEntity;
        }

        public BuyLevelSystem(EventService eventService)
        {
            eventService.AddListener<BuyResultEvent>(this);
        }

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
            int entity = world.NewEntity();
            ref var tryBuyEvent = ref world.GetPool<TryBuyEvent>().Add(entity);
            ref var business = ref world.GetPool<BusinessComponent>().Get(businessEntity);
            tryBuyEvent.Price = business.LevelUpPrice;
            tryBuyEvent.RequestId = entity;

            Request request = new Request();
            request.Id = entity;
            request.BusinessEntity = businessEntity;
            
            requests.Add(request);
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

            int entity = world.NewEntity();
            ref var levelUpEvent = ref world.GetPool<LevelUpEvent>().Add(entity);
            levelUpEvent.BusinessEntity = businessEntity;
        }

        public void OnEvent(ref BuyResultEvent result)
        {
            foreach (var request in requests)
            {
                if (request.Id == result.RequestId)
                {
                    LevelUp(request.BusinessEntity);
                    requests.Remove(request);
                    break;
                }
            }
        }
    }
}
