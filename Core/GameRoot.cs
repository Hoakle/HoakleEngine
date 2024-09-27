
using System.Collections;
using HoakleEngine.Core.Audio;
using HoakleEngine.Core.Game;
using HoakleEngine.Core.Graphics;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace HoakleEngine.Core
{
    public abstract class GameRoot : MonoBehaviour
    {
        protected IGameEngine _GameEngine;
        protected IGraphicsEngine _GraphicsEngine;

        [SerializeField] private GraphicsPool _GraphicsPool = null;
        [SerializeField] private Camera _Camera = null;
        public GraphicsPool GraphicsPool => _GraphicsPool;

        protected bool _IsPaused;

        [Inject]
        public void Inject(
            IGameEngine gameEngine,
            IGraphicsEngine graphicsEngine,
            AudioPlayer audioPlayer,
            ICameraProvider cameraProvider)
        {
            _GameEngine = gameEngine;
            _GraphicsEngine = graphicsEngine;
            cameraProvider.SetCamera(_Camera);
        }
        
        private void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            
        }

        void OnApplicationFocus(bool hasFocus)
        {
            _IsPaused = !hasFocus;
        }

        void OnApplicationPause(bool pauseStatus)
        {
            _IsPaused = pauseStatus;
        }
        
        public Coroutine StartEngineCoroutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        public void StopEngineCoroutine(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }
        
        #region GraphicsEngine

        [SerializeField] protected EventSystem _EventSystem = null;

        #endregion
    }
}
