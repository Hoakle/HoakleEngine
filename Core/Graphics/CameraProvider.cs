using UnityEngine;
using UniRx;

namespace HoakleEngine.Core.Graphics
{
    public interface ICameraProvider
    {
        IReadOnlyReactiveProperty<Camera> Camera { get; }
        void SetCamera(Camera camera);
    }
    
    public class CameraProvider : ICameraProvider
    {
        public IReadOnlyReactiveProperty<Camera> Camera
            => _Camera;
        
        private IReactiveProperty<Camera> _Camera = new ReactiveProperty<Camera>();
        
        public void SetCamera(Camera camera)
        {
            _Camera.Value = camera;
        }
    }
}
