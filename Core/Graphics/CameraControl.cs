using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace HoakleEngine.Core.Graphics
{
    public abstract class CameraControl : IUpdateable
    {
        protected List<Transform> _Targets; // All the targets the camera needs to encompass.
        protected Camera _Camera;
        public Camera Camera => _Camera;

        [Inject]
        public void Inject(ICameraProvider cameraProvider)
        {
            _Camera = cameraProvider.Camera.Value;
            cameraProvider.Camera
                .Subscribe(camera =>
                {
                    _Camera = camera;
                });
        }
        
        protected CameraControl ()
        {
            _Targets = new List<Transform>();
        }

        public virtual void AddTarget(Transform target)
        {
            _Targets.Add(target);
        }

        public virtual void RemoveTarget(Transform target)
        {
            _Targets.Remove(target);
        }

        public virtual void RemoveAllTarget()
        {
            _Targets.Clear();
        }

        public void Update(bool isPaused)
        {
            if (_Targets.Count == 0)
                return;
            
            // Move the camera towards a desired position.
            Move ();

            // Change the size of the camera based.
            Zoom ();
        }

        protected abstract void Move();

        protected abstract void Zoom();
        public abstract void SetStartPositionAndSize();
    }
}
