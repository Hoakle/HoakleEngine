using HoakleEngine.Core.Services.MiscService;
using UnityEngine;

namespace HoakleEngine.Core.Services
{
    public class MiscThirdPartyService : ThirdPartyService
    {
        private MiscThirdPartyActor _Actor;
        public override void Init()
        {
            _Actor = new AndroidMiscTPA();
            _Actor.Init();
        }

        public void OpenEmail()
        {
            _Actor.OpenEmail();
        }
    }
}
