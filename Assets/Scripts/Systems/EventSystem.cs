using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    public class EventSystem: IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld world;
        private EventService eventService;

        public EventSystem(EventService eventService)
        {
            this.eventService = eventService;
        }

        public void Init(IEcsSystems systems)
        {
            world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            ProcessEvents<BalanceChangedEvent>();
            ProcessEvents<LevelUpEvent>();
            ProcessEvents<UpgradePurchasedEvent>();
            ProcessEvents<TryBuyEvent>();
            ProcessEvents<IncreaseBalanceEvent>();
            ProcessEvents<BuyResultEvent>();
        }

        private void ProcessEvents<T>() where T : struct
        {
            if (!eventService.Listeners.TryGetValue(typeof(T), out var result)) return;

            var filter = world.Filter<T>().End();
            var pool = world.GetPool<T>();

            foreach (var entity in filter)
            {
                ref var evt = ref pool.Get(entity);

                foreach (var listenerObj in result)
                {
                    var listener = (IEcsEventListener<T>)listenerObj;
                    listener.OnEvent(ref evt);
                }

                world.DelEntity(entity);
            }
        }



    }
}
