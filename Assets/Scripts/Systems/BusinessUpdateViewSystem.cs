using Leopotam.EcsLite;
using UnityEngine;

namespace TestClickerEcs
{
    public class BusinessUpdateViewSystem : IEcsInitSystem, IEcsRunSystem, IEcsEventListener<LevelUpEvent>, IEcsEventListener<BalanceChangedEvent>, IEcsEventListener<UpgradePurchasedEvent>
    {
        private BusinessViewUpdateService businessViewUpdateService;

        private EcsWorld world;
        private EcsFilter businessComponents;
        private EcsFilter balanceFilter;

        public BusinessUpdateViewSystem(BusinessViewUpdateService businessViewUpdateService, EventService eventService)
        {
            this.businessViewUpdateService = businessViewUpdateService;

            eventService.AddListener<LevelUpEvent>(this);
            eventService.AddListener<BalanceChangedEvent>(this);
            eventService.AddListener<UpgradePurchasedEvent>(this);
        }


        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();

            businessComponents = world.Filter<BusinessComponent>().End();
            balanceFilter = world.Filter<BalanceComponent>().End();

            foreach (var entity in businessComponents)
            {
                ref var business = ref world.GetPool<BusinessComponent>().Get(entity);

                businessViewUpdateService.InitializeBusiness(business);
                businessViewUpdateService.SetProgressBar(business, 0);
                businessViewUpdateService.SetLevel(business, business.Level);
                businessViewUpdateService.SetProfit(business, business.Profit);


                int balance = GetBalance();

                bool levelUpButtonActive = balance >= business.LevelUpPrice;
                businessViewUpdateService.SetLevelUpPrice(business, business.LevelUpPrice);
                businessViewUpdateService.SetLevelUpButtonInterctable(business, levelUpButtonActive);


                for (int i = 0; i < business.Settings.Upgrades.Length; i++)
                {
                    bool isPurchased = business.UpgradeMultiplier[i] != 0;

                    if (isPurchased)
                    {
                        businessViewUpdateService.SetUpgradePurchased(business, i);
                        businessViewUpdateService.SetUpgradeButtonInterctable(business, i, false);
                    }
                    else
                    {
                        int price = business.Settings.Upgrades[i].ParamsUpgrade.Price;
                        businessViewUpdateService.SetUpgradePrice(business, i, price);
                        businessViewUpdateService.SetUpgradeButtonInterctable(business, i, balance >= price);
                    }


                                    
                }
            }
               
        }

        public void OnEvent(ref BalanceChangedEvent entityEvent)
        {
            foreach (var entityBusiness in businessComponents)
            {
                ref var business = ref world.GetPool<BusinessComponent>().Get(entityBusiness);

                int balance = GetBalance();

                bool levelUpButtonActive = balance >= business.LevelUpPrice;
                businessViewUpdateService.SetLevelUpPrice(business, business.LevelUpPrice);
                businessViewUpdateService.SetLevelUpButtonInterctable(business, levelUpButtonActive);


                for (int i = 0; i < business.Settings.Upgrades.Length; i++)
                {
                    int price = business.Settings.Upgrades[i].ParamsUpgrade.Price;
                    bool buttonActive = business.UpgradeMultiplier[i] == 0 && balance >= price;
                    businessViewUpdateService.SetUpgradeButtonInterctable(business, i, buttonActive);
                }


                businessViewUpdateService.SetProgressBar(business, business.ProfitProgress);

            }
        }

        public void OnEvent(ref LevelUpEvent levelUpEvent)
        {
            ref var business = ref world.GetPool<BusinessComponent>().Get(levelUpEvent.BusinessEntity);

            businessViewUpdateService.SetLevel(business, business.Level);
            businessViewUpdateService.SetProfit(business, business.Profit);
            businessViewUpdateService.SetLevelUpPrice(business, business.LevelUpPrice);
        }

        public void OnEvent(ref UpgradePurchasedEvent upgradePurchasedEvent)
        {
            ref var business = ref world.GetPool<BusinessComponent>().Get(upgradePurchasedEvent.BusinessEntity);

            businessViewUpdateService.SetUpgradePurchased(business, upgradePurchasedEvent.UpgradeId);
            businessViewUpdateService.SetProfit(business, business.Profit);
        }

        public void Run(IEcsSystems systems)
        {

            foreach (var entityBusiness in businessComponents)
            {
                ref var business = ref world.GetPool<BusinessComponent>().Get(entityBusiness);

                businessViewUpdateService.SetProgressBar(business, business.ProfitProgress);
            }

        }



        private int GetBalance()
        {
            int balance = 0;

            foreach (var balanceEntity in balanceFilter)
            {
                ref var balanceComponent = ref world.GetPool<BalanceComponent>().Get(balanceEntity);
                balance = balanceComponent.Value;
            }

            return balance;
        }

    }
}


