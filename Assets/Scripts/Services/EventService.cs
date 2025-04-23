using UnityEngine;
using System;
using System.Collections.Generic;

namespace TestClickerEcs
{

    public class EventService
    {
        private readonly Dictionary<Type, List<object>> listeners = new();

        public Dictionary<Type, List<object>> Listeners => listeners;

        public void AddListener<T>(IEcsEventListener<T> listener) where T : struct
        {
            if (!listeners.ContainsKey(typeof(T)))
            {
                listeners[typeof(T)] = new List<object>();
            }
            listeners[typeof(T)].Add(listener);
        }
    }

}
