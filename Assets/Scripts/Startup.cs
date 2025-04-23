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

        void Start()
        {
            SharedData sharedData = new SharedData(balanceView, rootBusinesses, prefabBusiness,businessWindowTextConfig, businesses);

            SaveLoadService saveLoadService = new SaveLoadService();
            EventService eventService = new EventService();
            BusinessViewUpdateService businessViewUpdateService = new BusinessViewUpdateService();

            world = new EcsWorld();
            systems = new EcsSystems(world, sharedData);

            systems
                .Add(new BusinessBuildSystem())
                .Add(new BalanceSaveLoadSystem(saveLoadService))
                .Add(new BusinessSaveLoadSystem(saveLoadService))               
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
        }
    }
}

