using System;
using System.Collections.Generic;
using HoakleEngine.Core.Game;
using UnityEngine;

namespace HoakleEngine.Core.Services
{
    public class ServicesContainer
    {
        private Dictionary<Type, ThirdPartyService> _ServiceCache = new Dictionary<Type, ThirdPartyService>();

        public ServicesContainer()
        {
            _ServiceCache.Add(typeof(MiscThirdPartyService), new MiscThirdPartyService());
        }

        public void Init()
        {
            foreach (var service in _ServiceCache)
            {
                service.Value.Init();
            }
        }
        public T GetService<T>() where T : ThirdPartyService
        {
            return (T) _ServiceCache[typeof(T)];
        }
    }
}
