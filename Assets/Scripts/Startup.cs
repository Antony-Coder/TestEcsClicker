using Leopotam.EcsLite;
using Leopotam.EcsLite.ExtendedSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    public class Startup : MonoBehaviour
    {
        [SerializeField] private Transform rootBusinesses;
        [SerializeField] private BusinessWindowTextConfig businessWindowTextConfig;
        [SerializeField] private BusinessView prefabBusiness;
        [SerializeField] private BalanceView balanceView;
        [SerializeField] private BusinessSettings[] businesses;

        private EcsWorld world;
        private IEcsSystems systems;
        private IEcsSystems saveSystems;

        void Start()
        {
            SharedData sharedData = new SharedData(balanceView, rootBusinesses, prefabBusiness,businessWindowTextConfig, businesses);
            BusinessViewUpdateService businessViewUpdateService = new BusinessViewUpdateService();
            SaveLoadService saveLoadService = new SaveLoadService();

            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);
            saveSystems = new EcsSystems(world);

            systems
                .Add(new BusinessBuildSystem())
                .Add(new BusinessLoadSystem(saveLoadService))
                .Add(new BalanceLoadSystem(saveLoadService))               
                .Add(new BuyUpgradeSystem())
                .Add(new BuyLevelSystem())
                .Add(new ProfitSystem())
                .Add(new BalanceSystem())
                .Add(new BuyUpgradeHandlerSystem())
                .Add(new BuyLevelHandlerSystem())
                .Add(new BusinessUpdateViewSystem(businessViewUpdateService))
                .Add(new BalanceUpdateViewSystem())
                .DelHere<LevelUpEvent>()
                .DelHere<BalanceChangedEvent>()
                .DelHere<BuyResultEvent>()
                .DelHere<IncreaseBalanceEvent>()
                .DelHere<UpgradeEvent>()
                .DelHere<TryBuyEvent>()
                .Init();


            saveSystems
                .Add(new BalanceSaveSystem(saveLoadService))
                .Add(new BusinessSaveSystem(saveLoadService))
                .Init();
        }

        void Update()
        {
            systems?.Run();
        }

        void OnDestroy()
        {
            if (systems != null)
            {
                saveSystems.Destroy();
                systems.Destroy();
                saveSystems = null;
                systems = null;
            }
            if (world != null)
            {
                world.Destroy();
                world = null;
            }

        }


        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                Save();
            }
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Save();
            }

        }

        private void Save()
        {
            saveSystems?.Run();
        }


    }
}

