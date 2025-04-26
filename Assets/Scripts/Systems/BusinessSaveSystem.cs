using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    public class BusinessSaveSystem : IEcsRunSystem
    {
        private SaveLoadService saveLoadService;

        public BusinessSaveSystem(SaveLoadService saveLoadService)
        {
            this.saveLoadService = saveLoadService;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            foreach (var entity in world.Filter<BusinessComponent>().End())
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
