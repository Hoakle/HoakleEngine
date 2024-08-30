using System;

namespace HoakleEngine.Core.Services.AdsServices
{
    public interface AdsThirdPartyActor : ThirdPartyActor
    {
        public Action<string> OnLoaded { get; set; }
        public Action<string, bool> OnShowComplete { get; set; }
        public void Load(string rewardedAd);
        public void Show(string rewardedAd);
    }
}
