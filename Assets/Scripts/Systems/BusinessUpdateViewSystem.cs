using Leopotam.EcsLite;
using UnityEngine;

namespace TestClickerEcs
{
    public class BusinessUpdateViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private BusinessViewUpdateService businessViewUpdateService;

        private EcsWorld world;


        public BusinessUpdateViewSystem(BusinessViewUpdateService businessViewUpdateService)
        {
            this.businessViewUpdateService = businessViewUpdateService;
        }


        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();


            foreach (var entity in world.Filter<BusinessComponent>().End())
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






        public void Run(IEcsSystems systems)
        {
            foreach (var entity in world.Filter<LevelUpEvent>().End())
            {
                ref var result = ref world.GetPool<BuyResultEvent>().Get(entity);

                if (result.Success)
                {
                    ref var levelUpEvent = ref world.GetPool<LevelUpEvent>().Get(entity);
                    ref var business = ref world.GetPool<BusinessComponent>().Get(levelUpEvent.BusinessEntity);

                    businessViewUpdateService.SetLevel(business, business.Level);
                    businessViewUpdateService.SetProfit(business, business.Profit);
                    businessViewUpdateService.SetLevelUpPrice(business, business.LevelUpPrice);
                }

            }


            foreach (var entity in world.Filter<UpgradeEvent>().End())
            {

                ref var result = ref world.GetPool<BuyResultEvent>().Get(entity);

                if (result.Success)
                {
                    ref var upgradePurchasedEvent = ref world.GetPool<UpgradeEvent>().Get(entity);

                    ref var business = ref world.GetPool<BusinessComponent>().Get(upgradePurchasedEvent.BusinessEntity);

                    businessViewUpdateService.SetUpgradePurchased(business, upgradePurchasedEvent.UpgradeId);
                    businessViewUpdateService.SetProfit(business, business.Profit);
                }


            }


            foreach (var entityEvent in world.Filter<BalanceChangedEvent>().End())
            {
                foreach (var entity in world.Filter<BusinessComponent>().End())
                {
                    ref var business = ref world.GetPool<BusinessComponent>().Get(entity);

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

                }

            }

            foreach (var entity in world.Filter<BusinessComponent>().End())
            {
                ref var business = ref world.GetPool<BusinessComponent>().Get(entity);
                businessViewUpdateService.SetProgressBar(business, business.ProfitProgress);
            }

        }



        private int GetBalance()
        {
            int balance = 0;

            foreach (var balanceEntity in world.Filter<BalanceComponent>().End())
            {
                ref var balanceComponent = ref world.GetPool<BalanceComponent>().Get(balanceEntity);
                balance = balanceComponent.Value;
            }

            return balance;
        }

    }
}


