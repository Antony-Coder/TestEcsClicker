using Leopotam.EcsLite;
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
        private EventService eventService;
        private SaveLoadService saveLoadService;


        void Start()
        {
            SharedData sharedData = new SharedData(balanceView, rootBusinesses, prefabBusiness,businessWindowTextConfig, businesses);


            BusinessViewUpdateService businessViewUpdateService = new BusinessViewUpdateService();

            eventService = new EventService();
            saveLoadService = new SaveLoadService();

            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);

            systems
                .Add(new BusinessBuildSystem())
                .Add(new BusinessSaveLoadSystem(saveLoadService))
                .Add(new BalanceSaveLoadSystem(saveLoadService))               
                .Add(new BuyUpgradeSystem(eventService))
                .Add(new BuyLevelSystem(eventService))
                .Add(new ProfitSystem())
                .Add(new BalanceSystem(eventService))
                .Add(new BusinessUpdateViewSystem( businessViewUpdateService,eventService))
                .Add(new BalanceUpdateViewSystem(eventService))
                .Add(new EventSystem(eventService))
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
                systems.Destroy();
                systems = null;
            }
            if (world != null)
            {
                world.Destroy();
                world = null;
            }

            eventService.Destroy();
            saveLoadService.Destroy();
        }

        private void Save()
        {
            if (saveLoadService == null) return;
            saveLoadService.SaveAll();
        }



        private void OnApplicationFocus(bool hasFocus)
        {
            Save();
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
    }
}

