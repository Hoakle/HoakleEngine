using System;
using System.Collections.Generic;

namespace HoakleEngine.Core.Communication
{
    public class EventBus
    {
        private Dictionary<Type, List<Action<object>>> eventSubscriptions = new Dictionary<Type, List<Action<object>>>();
        
        public void Subscribe<T>(Action<T> handler) where T : class
        {
            Type eventType = typeof(T);

            if (!eventSubscriptions.ContainsKey(eventType))
            {
                eventSubscriptions[eventType] = new List<Action<object>>();
            }

            eventSubscriptions[eventType].Add((obj) => handler(obj as T));
        }
        
        public void Publish<T>(T eventData) where T : class
        {
            Type eventType = typeof(T);

            if (eventSubscriptions.ContainsKey(eventType))
            {
                foreach (var handler in eventSubscriptions[eventType])
                {
                    handler(eventData);
                }
            }
        }
    }
}
