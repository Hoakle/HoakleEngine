using System.Collections.Generic;
using UnityEngine;

namespace HoakleEngine.Core.Localization
{
    [CreateAssetMenu(fileName = "LocalizationDataBase", menuName = "HoakleEngine/Localization/LocalizationDataBase")]
    public class LocalizationDataBase : ScriptableObject
    {
        public SystemLanguage DefaultLanguage => _DefaultLanguage;
        public List<string> Keys => _Keys;
        public LanguageData SelectedLanguage => _SelectedLanguage;
            
        [SerializeField] private SystemLanguage _DefaultLanguage;
        [SerializeField] private List<string> _Keys;
        [SerializeField] public List<LanguageData> _Language;

        private Dictionary<SystemLanguage, string> _AvailableLanguages;
        private LanguageData _SelectedLanguage;
        public Dictionary<SystemLanguage, string> GetAvailableLanguages()
        {
            if (_AvailableLanguages == null)
                InitLanguages();

            return _AvailableLanguages;
        }

        private void InitLanguages()
        {
            _AvailableLanguages = new Dictionary<SystemLanguage, string>();
            foreach (var language in _Language)
            {
                _AvailableLanguages.Add(language.Language, language.Name);
            }
        }

        public void SetLanguage(SystemLanguage language)
        {
            _SelectedLanguage = _Language.Find(l => l.Language == language);

            if (_SelectedLanguage == null)
                _SelectedLanguage = _Language.Find(l => l.Language == _DefaultLanguage);
        }
    }
}
