using UnityEngine;

namespace HoakleEngine.Core.UI.Skin
{
    [CreateAssetMenu(fileName = "UISkinData", menuName = "Game Data/UISkin/UISkinData")]
    public class UISkinData : ScriptableObject
    {
        public Color HeaderColor;
        public Color BodyColor;
    }
}
