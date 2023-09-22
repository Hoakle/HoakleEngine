using System.Collections.Generic;
using HoakleEngine.Core.Game;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HoakleEngine.Core.Audio
{
    public class AudioPlayer : IUpdateable
    {
        private static AudioPlayer _Instance;
        public static AudioPlayer Instance
        {
            get
            {
                _Instance ??= new AudioPlayer();
                return _Instance;
            }
        }

        private Dictionary<AudioKeys, AudioSettings> _AudioDic;
        private Transform _AudioPlayerTransform;

        private Dictionary<AudioKeys, List<AudioSource>> _CurrentlyPlaying;

        private SettingsGameSave _SettingsGameSave;
        public void Init(SettingsGameSave settingsGameSave, AudioList list, Transform parent)
        {
            _SettingsGameSave = settingsGameSave;
            _SettingsGameSave.OnMusicToogle += ToogleMusic;
            _SettingsGameSave.OnSfxToogle += ToogleSfx;
            
            _CurrentlyPlaying = new Dictionary<AudioKeys, List<AudioSource>>();
            _AudioDic = new Dictionary<AudioKeys, AudioSettings>();
            foreach (var audio in list._AudioList)
            {
                _AudioDic.Add(audio.Key, audio);
            }

            _AudioPlayerTransform = parent;
        }
        
        public void Play(AudioKeys key)
        {
            if (IsAlreadyPlaying(key) && _AudioDic[key].PlayOnlyOnce)
                return;

            GameObject go = new GameObject();
            go.name = "AudioSource_" + key;
            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.clip = _AudioDic[key].AudioClip;
            audioSource.loop =  _AudioDic[key].IsLooping;
            audioSource.volume =  GetVolume(_AudioDic[key]);
            go.transform.parent = _AudioPlayerTransform;
            audioSource.Play();

            if (!_CurrentlyPlaying.ContainsKey(key))
                _CurrentlyPlaying.Add(key, new List<AudioSource>());

            _CurrentlyPlaying[key].Add(audioSource);
        }

        private float GetVolume(AudioSettings audioSettings)
        {
            if (audioSettings.IsMusic)
                return _SettingsGameSave.HasMusic ? audioSettings.Volume : 0f;
            
            return _SettingsGameSave.HasSfx ? audioSettings.Volume : 0f;
        }

        public void Stop(AudioKeys key)
        {
            foreach (var audio in _CurrentlyPlaying[key])
            {
                audio.Stop();
                Object.Destroy(audio.gameObject);
            }
            
            _CurrentlyPlaying[key].Clear();
        }

        public bool IsAlreadyPlaying(AudioKeys key)
        {
            if (_CurrentlyPlaying.ContainsKey(key) && _CurrentlyPlaying[key].Count > 0)
                return true;

            return false;
        }
        
        private void ToogleMusic(bool isActive)
        {
            Debug.LogError(_SettingsGameSave.HasMusic);
            foreach (var audioList in _CurrentlyPlaying)
            {
                if(_AudioDic[audioList.Key].IsMusic)
                    foreach (var audio in audioList.Value)
                    {
                        audio.volume = GetVolume(_AudioDic[audioList.Key]);
                    }
            }
        }

        private void ToogleSfx(bool isActive)
        {
            foreach (var audioList in _CurrentlyPlaying)
            {
                if(!_AudioDic[audioList.Key].IsMusic)
                    foreach (var audio in audioList.Value)
                    {
                        audio.volume = GetVolume(_AudioDic[audioList.Key]);
                    }
            }
        }
        
        public void Update(bool isPaused)
        {
            if (isPaused)
                return;
            
            foreach (var audioList in _CurrentlyPlaying)
            {
                for(int i = audioList.Value.Count - 1; i >= 0; i--)
                {
                    if (!audioList.Value[i].isPlaying)
                    {
                        Object.Destroy(audioList.Value[i].gameObject);
                        audioList.Value.RemoveAt(i);
                    }
                }
            }
        }
    }
    
    public enum AudioKeys
    {
        //Effect
        Click = 0,
        Whoosh = 1,
        CoinCollect = 2,
        CoinCollect_02 = 3,
        Jump = 4,
        BonusCollect = 5,
        
        //Music
        RainLoop = 100,
        MainLevelLoop = 101,
    }

    
}
