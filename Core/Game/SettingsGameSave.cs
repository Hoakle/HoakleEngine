using System;
using UniRx;
using UnityEngine;

namespace HoakleEngine.Core.Game
{
    [Serializable]
    public class SettingsGameSave : GameSaveHandler<SettingsData>
    {
        public IReadOnlyReactiveProperty<bool> HasMusic
            => _HasMusic;

        public IReadOnlyReactiveProperty<bool> HasSfx
            => _HasSfx;

        public SystemLanguage Language
        {
            get => (SystemLanguage) _Data.Language;
            set
            {
                _Data.Language = (int) value;
                Save();
            }
        }

        private IReactiveProperty<bool> _HasMusic = new ReactiveProperty<bool>();
        private IReactiveProperty<bool> _HasSfx = new ReactiveProperty<bool>();
        
        public SettingsGameSave() : base("SettingsGameSave")
        {
            
        }
        
        protected override void BuildData()
        {
            base.BuildData();
            if (!_GameSaveService.Exist<SettingsData>(_Identifier))
            {
                _Data.HasMusic = true;
                _Data.HasSfx = true;
            }
            
            _HasMusic.Value = _Data.HasMusic;
            _HasSfx.Value = _Data.HasSfx;
        }

        public override void Save()
        {
            _Data.HasMusic = _HasMusic.Value;
            _Data.HasSfx = _HasSfx.Value;
            base.Save();
        }

        public void ToggleMusic(bool hasMusic)
        {
            _HasMusic.Value = hasMusic;
            Save();
        }
        
        public void ToggleSfx(bool hasSfx)
        {
            _HasSfx.Value = hasSfx;
            Save();
        }
    }
    
    [Serializable]
    public struct SettingsData
    {
        public bool HasMusic;
        public bool HasSfx;
        public int Language;
    }
}
