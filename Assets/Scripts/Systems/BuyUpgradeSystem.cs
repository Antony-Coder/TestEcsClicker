using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    public class BuyUpgradeSystem : IEcsInitSystem, IEcsEventListener<BuyResultEvent>
    {
        private EcsWorld world;

        private HashSet<Request> requests = new HashSet<Request>();

        private struct Request
        {
            public int Id;
            public int UpgradeId;
            public int BusinessEntity;
        }

        public BuyUpgradeSystem(EventService eventService)
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
                

                for (int i = 0; i < business.View.Upgrades.Length; i++)
                {
                    int upgradeId = i;
                    business.View.Upgrades[i].Button.onClick.AddListener(() => TryBuy(businessEntity, upgradeId));
                }

            }
        }

        private void TryBuy(int businessEntity, int upgradeId)
        {
            int entity = world.NewEntity();
            ref var tryBuyEvent = ref world.GetPool<TryBuyEvent>().Add(entity);
            ref var business = ref world.GetPool<BusinessComponent>().Get(businessEntity);
            tryBuyEvent.Price = business.LevelUpPrice;
            tryBuyEvent.RequestId = entity;

            Request request = new Request();
            request.Id = entity;
            request.UpgradeId = upgradeId;
            request.BusinessEntity = businessEntity;

            requests.Add(request);
        }



        private void Upgrade(Request request)
        {
            int entity = world.NewEntity();
            ref var upgradePurchasedEvent = ref world.GetPool<UpgradePurchasedEvent>().Add(entity);
            upgradePurchasedEvent.BusinessEntity = request.BusinessEntity;
            upgradePurchasedEvent.UpgradeId = request.UpgradeId;

            ref var business = ref world.GetPool<BusinessComponent>().Get(request.BusinessEntity);

            business.UpgradeMultiplier[request.UpgradeId] = business.Settings.Upgrades[request.UpgradeId].ParamsUpgrade.Procent / 100f;

            float multiplier = 0;
            foreach (var m in business.UpgradeMultiplier)
            {
                multiplier += m;
            }
            business.Profit = (int)(business.Level * business.Settings.Params.Profit * (1 + multiplier));
        }

        public void OnEvent(ref BuyResultEvent result)
        {
            foreach (var request in requests)
            {
                if (request.Id == result.RequestId)
                {
                    Upgrade(request);
                    requests.Remove(request);
                    break;
                }
            }
        }
    }
}
