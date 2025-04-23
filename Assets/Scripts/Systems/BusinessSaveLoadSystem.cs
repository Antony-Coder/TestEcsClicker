using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    public class BusinessSaveLoadSystem : IEcsInitSystem,  IEcsDestroySystem
    {
        private SaveLoadService saveLoadService;
        private EcsWorld world;
        public BusinessSaveLoadSystem(SaveLoadService saveLoadService)
        {
            this.saveLoadService = saveLoadService;
            saveLoadService.SaveEvent += Save;
        }


        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();

            var filter = world.Filter<BusinessComponent>().End();

            int count = 0;

            foreach (var entity in filter)
            {
                string saveKey = "BusinessSave" + count;

                ref var business = ref world.GetPool<BusinessComponent>().Get(entity);

                BusinessSave saveDefault = new BusinessSave();
                saveDefault.Level = business.Settings.Params.Purchased ? 1:0;
                saveDefault.UpgradePurchased = new bool[business.Settings.Upgrades.Length];

                BusinessSave save = saveLoadService.Load<BusinessSave>(saveKey, saveDefault);



   


                business.Id = count;
                business.Level = save.Level;
                business.LevelUpPrice = (business.Level + 1) * business.Settings.Params.Price;

                for (int i = 0; i < business.UpgradeMultiplier.Length; i++)
                {
                    if (save.UpgradePurchased[i])
                    {
                        business.UpgradeMultiplier[i] = business.Settings.Upgrades[i].ParamsUpgrade.Procent / 100f;
                    }
                    else
                    {
                        business.UpgradeMultiplier[i] = 0;
                    }
                }

                float multiplier = 0;
                foreach (var m in business.UpgradeMultiplier)
                {
                    multiplier += m;
                }

                business.Profit = (int)(business.Level * business.Settings.Params.Profit * (1 + multiplier));


                count++;
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            Save();
        }



        public void Save()
        {
            if (world == null) return;

            Debug.Log("SAVE");

            var filter = world.Filter<BusinessComponent>().End();

            foreach (var entity in filter)
            {
                ref var business = ref world.GetPool<BusinessComponent>().Get(entity);
                string saveKey = "BusinessSave" + business.Id;

                BusinessSave save = new BusinessSave();
                save.Level = business.Level;
                save.UpgradePurchased = new bool[business.UpgradeMultiplier.Length];

                for (int i = 0; i < business.UpgradeMultiplier.Length; i++)
                {
                    save.UpgradePurchased[i] = business.UpgradeMultiplier[i] != 0 ? true : false;
                }

                saveLoadService.Save<BusinessSave>(saveKey, save);
            }
        }
    }
}
