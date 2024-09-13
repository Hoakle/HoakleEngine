using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace HoakleEngine.Core.Localization
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string _Key;
        [SerializeField] private TextMeshProUGUI _Text;
        
        private ILocalizationProvider _LocalizationProvider;
        private string[] _Parameters;
        
        [Inject]
        public void Inject(ILocalizationProvider localizationProvider)
        {
            _LocalizationProvider = localizationProvider;
            _LocalizationProvider.OnLanguageChange += Translate;
            
            if(_LocalizationProvider.IsInitialized)
                Translate();
        }
        
        
        public void SetKey(string key)
        {
            _Key = key;
            Translate();
        }
        
        public void SetParameters(params string[] parameters)
        {
            _Parameters = parameters;
            Translate();
        }
        
        private void Translate()
        {
            _Text.text = _LocalizationProvider.Translate(_Key, _Parameters);
        }

        public void OnDestroy()
        {
            _LocalizationProvider.OnLanguageChange -= Translate;
        }
    }
}
