using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HoakleEngine.Core.Audio
{
    public class PlayOnClick : MonoBehaviour
    {
        [SerializeField] private Button _Button = null;
        [SerializeField] private AudioKeys _Key = AudioKeys.Click;

        private AudioPlayer _AudioPlayer;

        [Inject]
        public void Inject(AudioPlayer audioPlayer)
        {
            _AudioPlayer = audioPlayer;
        }
        
        private void PlayAudio()
        {
            _AudioPlayer.Play(_Key);
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
