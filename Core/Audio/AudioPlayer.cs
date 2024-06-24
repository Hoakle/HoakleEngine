using System.Collections.Generic;
using HoakleEngine.Core.Game;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace HoakleEngine.Core.Audio
{
    public class AudioPlayer : ITickable
    {
        private Dictionary<AudioKeys, AudioSettings> _AudioDic;
        private Transform _AudioPlayerTransform;

        private Dictionary<AudioKeys, List<AudioSource>> _CurrentlyPlaying;

        private SettingsGameSave _SettingsGameSave;

        public AudioPlayer(AudioList list, Transform parent)
        {
            _CurrentlyPlaying = new Dictionary<AudioKeys, List<AudioSource>>();
            _AudioDic = new Dictionary<AudioKeys, AudioSettings>();
            foreach (var audio in list._AudioList)
            {
                _AudioDic.Add(audio.Key, audio);
            }

            _AudioPlayerTransform = parent;
        }
        
        [Inject]
        public void Inject(SettingsGameSave settingsGameSave)
        {
            _SettingsGameSave = settingsGameSave;
            _SettingsGameSave.HasMusic.Subscribe(ToogleMusic);
            _SettingsGameSave.HasSfx.Subscribe(ToogleSfx);
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
                return _SettingsGameSave.HasMusic.Value ? audioSettings.Volume : 0f;
            
            return _SettingsGameSave.HasSfx.Value ? audioSettings.Volume : 0f;
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
        
        public void Tick()
        {
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
        GameOver = 6,
        
        //Music
        RainLoop = 100,
        MainLevelLoop = 101,
    }

    
}
