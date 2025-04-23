using Leopotam.EcsLite;
using UnityEngine;

namespace TestClickerEcs
{
    public class BusinessBuildSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            SharedData sharedData = systems.GetShared<SharedData>();

            for(int i = 0; i < sharedData.Businesses.Length; i++)
            {
                BusinessView businessView = GameObject.Instantiate(sharedData.PrefabBusiness, Vector3.zero, Quaternion.identity, sharedData.RootBusinesses);

                var entity = systems.GetWorld().NewEntity();
                ref var business = ref world.GetPool<BusinessComponent>().Add(entity);


                business.Id = i;
                business.Level = 0;
                business.UpgradeMultiplier = new float[sharedData.Businesses[i].Upgrades.Length];
                business.Settings = sharedData.Businesses[i];
                business.View = businessView;
  
            }

        }


    }
}

