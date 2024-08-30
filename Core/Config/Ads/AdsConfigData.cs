using UnityEngine;

namespace HoakleEngine.Core.Config.Ads
{
    [CreateAssetMenu(fileName = "AdsConfigData", menuName = "Game Data/Config/AdsConfigData")]
    public class AdsConfigData : ScriptableObject
    {
        public string Id;
        public AdsType Type;
        public int Value;
    }

    public enum AdsType
    {
        SIMPLE = 0,
        MULTIPLICATOR = 1,
    }
}
