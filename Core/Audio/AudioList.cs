using System;
using System.Collections.Generic;
using UnityEngine;

namespace HoakleEngine.Core.Audio
{
    [CreateAssetMenu(fileName = "AudioList", menuName = "Game Audio/AudioList")]
    public class AudioList : ScriptableObject
    {
        public List<AudioSettings> _AudioList = new List<AudioSettings>();
    }
    
    
    [Serializable]
    public class AudioSettings
    {
        public AudioKeys Key;
        public AudioClip AudioClip;
        public bool IsMusic;
        public bool IsLooping;
        public bool PlayOnlyOnce;
        
        [Range(0f, 1f)]
        public float Volume;
    }
}
