using System;
using System.Collections;
using System.Collections.Generic;
using HoakleEngine.Core.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace HoakleEngine
{
    public class PlayOnClick : MonoBehaviour
    {
        [SerializeField] private Button _Button = null;
        [SerializeField] private AudioKeys _Key = AudioKeys.Click;

        private void PlayAudio()
        {
            AudioPlayer.Instance.Play(_Key);    
        }
        
        public void Awake()
        {
            _Button.onClick.AddListener(PlayAudio);
        }

        public void OnDestroy()
        {
            _Button.onClick.AddListener(PlayAudio);
        }
    }
}
