using UnityEngine;
using Zenject;

namespace HoakleEngine.Core.Audio
{
    public class AudioInstaller : MonoInstaller
    {
        [SerializeField] private AudioList _AudioList = null;
        [SerializeField] private Transform _AudioEmitter = null;
        public override void InstallBindings()
        {
            Container.Bind<AudioPlayer>().AsSingle().WithArguments(_AudioList, _AudioEmitter);
            
        }
    }
}
