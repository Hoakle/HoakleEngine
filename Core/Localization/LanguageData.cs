using System;
using System.Collections.Generic;
using UnityEngine;

namespace HoakleEngine.Core.Localization
{
    [CreateAssetMenu(fileName = "LanguageData", menuName = "HoakleEngine/Localization/LanguageData")]
    public class LanguageData : ScriptableObject
    {
        [SerializeField] public string Name;
        [SerializeField] public SystemLanguage Language;
        [SerializeField] public List<string> Translations;
    }
}
