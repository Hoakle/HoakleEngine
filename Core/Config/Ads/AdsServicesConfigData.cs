using System.Collections.Generic;
using RetroRush.Config;
using UnityEngine;

namespace HoakleEngine.Core.Config.Ads
{
    [CreateAssetMenu(fileName = "AdsServicesConfigData", menuName = "Game Data/Config/AdsServicesConfigData")]
    public class AdsServicesConfigData : ScriptableObject
    {
        [SerializeField]
        private List<AdsConfigData> _AdsConfig = null;
        
        //
        public static string RVADS_CONTINUE = "RVADS_CONTINUE";
        public static string RVADS_COIN = "RVADS_COIN";

        public AdsConfigData GetAdsConfig(string key)
        {
            return _AdsConfig.Find(c => c.Id == key);
        }
    }
}
