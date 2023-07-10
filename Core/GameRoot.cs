using System.Collections;
using System.Collections.Generic;
using HoakleEngine.Core.Communication;
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

        protected EventBus EventBus;
        
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

        [SerializeField] protected EventSystem m_EventSystem = null;

        [SerializeField] protected Camera m_Camera;

        #endregion
    }
}
