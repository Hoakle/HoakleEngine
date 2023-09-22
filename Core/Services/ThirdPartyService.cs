using System.Collections.Generic;
using UnityEngine;

namespace HoakleEngine.Core.Services
{
    public abstract class ThirdPartyService
    {
        protected List<ThirdPartyActor> _Actors = new List<ThirdPartyActor>();
        public abstract void Init();
    }
}
