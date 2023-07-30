using System;
using System.Collections.Generic;
using UnityEngine;

namespace HoakleEngine.Core.Communication
{
    public class EventBus
    {
        private static EventBus _Instance;
        public static EventBus Instance
        {
            get
            {
                _Instance ??= new EventBus();
                return _Instance;
            }
        }
        private Dictionary<EngineEventType, List<Delegate>> eventSubscriptions = new Dictionary<EngineEventType, List<Delegate>>();
        
        public void Subscribe<T>(EngineEventType eventType, Action<T> handler) where T : class
        {
            if (!eventSubscriptions.ContainsKey(eventType))
            {
                eventSubscriptions[eventType] = new List<Delegate>();
            }
            
            eventSubscriptions[eventType].Add(handler);
        }
        
        public void Subscribe(EngineEventType eventType, Action handler)
        {
            if (!eventSubscriptions.ContainsKey(eventType))
            {
                eventSubscriptions[eventType] = new List<Delegate>();
            }
            
            eventSubscriptions[eventType].Add(handler);
        }
        
        public void UnSubscribe<T>(EngineEventType eventType, Action<T> handler) where T : class
        {
            eventSubscriptions[eventType].Remove(handler);
        }
        
        public void UnSubscribe(EngineEventType eventType, Action handler)
        {
            eventSubscriptions[eventType].Remove(handler);
        }
        
        public void Publish<T>(EngineEventType eventType, T eventData) where T : class
        {
            if (eventSubscriptions.ContainsKey(eventType))
            {
                foreach (var handler in eventSubscriptions[eventType])
                {
                    if(handler is Action<T> action)
                        action.Invoke(eventData);
                }
            }
        }
        
        public void Publish(EngineEventType eventType)
        {
            if (eventSubscriptions.ContainsKey(eventType))
            {
                foreach (var handler in eventSubscriptions[eventType])
                {
                    if(handler is Action action)
                        action.Invoke();
                }
            }
        }
    }

    public enum EngineEventType
    {
        SpeedBonus,
        Coin,
        SpeedBonusFadeOut
    }
}