using System;
using UnityEngine;

namespace HoakleEngine.Core.Game
{
    [Serializable]
    public class SettingsGameSave : GameSave
    {
        [SerializeField] private bool _HasMusic = true;
        [SerializeField] private bool _HasSfx = true;

        public Action<bool> OnMusicToogle;
        public Action<bool> OnSfxToogle;
        
        public SettingsGameSave()
        {
            SaveName = "SettingsGameSave";
        }
        
        public bool HasMusic
        {
            get => _HasMusic;
            set
            {
                _HasMusic = value;
                OnMusicToogle?.Invoke(value);
            }
        }
        
        public bool HasSfx
        {
            get => _HasSfx;
            set
            {
                _HasSfx = value;
                OnSfxToogle?.Invoke(value);
            }
        }
        public override void Init()
        {
            
        }
    }
}
