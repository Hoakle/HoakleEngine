using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HoakleEngine.Core.Services
{
    public abstract class ThirdPartyService : IInitializable
    {
        protected List<ThirdPartyActor> _Actors = new List<ThirdPartyActor>();
        public abstract void Initialize();
    }
}
