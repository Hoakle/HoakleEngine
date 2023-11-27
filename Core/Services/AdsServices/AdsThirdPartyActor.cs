using System;
using System.Collections;
using System.Collections.Generic;
using HoakleEngine.Core.Services;
using UnityEngine;

namespace HoakleEngine
{
    public interface AdsThirdPartyActor : ThirdPartyActor
    {
        public Action<string> OnLoaded { get; set; }
        public Action<string, bool> OnShowComplete { get; set; }
        public void Load(string rewardedAd);
        public void Show(string rewardedAd);
    }
}
