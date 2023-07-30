using System.Collections;
using HoakleEngine.Core.Game;
using HoakleEngine.Core.Graphics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HoakleEngine
{
    public abstract class GameRoot : MonoBehaviour
    {
        protected GameEngine GameEngine;
        protected GraphicsEngine GraphicsEngine;
        
        [SerializeField] private ConfigContainer _ConfigContainer;
        public ConfigContainer ConfigContainer => _ConfigContainer;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Init();
        }

        protected abstract void Init();
        
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

        [SerializeField] protected Camera _Camera;
        public Camera Camera => _Camera;

        #endregion
    }
}
