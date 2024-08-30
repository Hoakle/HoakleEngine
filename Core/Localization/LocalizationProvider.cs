using System;
using System.Collections.Generic;
using HoakleEngine.Core.Game;
using UnityEngine;
using Zenject;

namespace HoakleEngine.Core.Localization
{
    public interface ILocalizationProvider
    {
        public Dictionary<SystemLanguage, string> AvailableLanguage { get; }
        public void SetLanguage(SystemLanguage language);
        public string Translate(string key, params string[] parameters);
        public Action OnLanguageChange { get; set; }
    }
    
    public class LocalizationProvider : ILocalizationProvider
    {
        public Dictionary<SystemLanguage, string> AvailableLanguage
            => _LocalizationDataBase.GetAvailableLanguages();

        public Action OnLanguageChange
        {
            get => _OnLanguageChange;
            set => _OnLanguageChange = value;
        }
        
        private LocalizationDataBase _LocalizationDataBase;
        private Dictionary<string, string> _LanguageKeys;
        private Action _OnLanguageChange;
        private SettingsGameSave _SettingsGameSave;

        [Inject]
        public void Inject(LocalizationDataBase localizationDataBase, SettingsGameSave settingsGameSave)
        {
            _LocalizationDataBase = localizationDataBase;
            _SettingsGameSave = settingsGameSave;
            Initialize();
        }
        
        public void SetLanguage(SystemLanguage language)
        {
            _LocalizationDataBase.SetLanguage(language);
            
            for (int i = 0; i < _LocalizationDataBase.Keys.Count; i++)
            {
                _LanguageKeys[_LocalizationDataBase.Keys[i]] = _LocalizationDataBase.SelectedLanguage.Translations[i];
            }
            
            _OnLanguageChange?.Invoke();
        }

        public string Translate(string key, params string[] parameters)
        {
            if (!_LanguageKeys.TryGetValue(key, out var translation))
                return key;

            if (parameters == null) 
                return translation;

            for (int i = 0; i < parameters.Length; i++)
            {
                translation = translation.Replace($"{{{i}}}", parameters[i]);
            }
            
            return translation;
        }

        private void Initialize()
        {
            _LanguageKeys ??= new Dictionary<string, string>();
            foreach (var key in _LocalizationDataBase.Keys)
            {
                _LanguageKeys.Add(key, "");
            }

            if (_SettingsGameSave.Language == SystemLanguage.Afrikaans)
            {
                _SettingsGameSave.Language = Application.systemLanguage;
            }
            
            SetLanguage(_SettingsGameSave.Language);

            _SettingsGameSave.Language = _LocalizationDataBase.SelectedLanguage.Language;
        }
    }
}
