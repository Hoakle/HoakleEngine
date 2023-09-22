using System.Collections;
using HoakleEngine.Core.Audio;
using HoakleEngine.Core.Config;
using HoakleEngine.Core.Game;
using HoakleEngine.Core.Graphics;
using HoakleEngine.Core.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HoakleEngine.Core
{
    public abstract class GameRoot : MonoBehaviour
    {
        protected GameEngine GameEngine;
        protected GraphicsEngine GraphicsEngine;

        [SerializeField] private GraphicsPool _GraphicsPool = null;
        [SerializeField] private ConfigContainer _ConfigContainer = null;
        [SerializeField] private AudioList _AudioList = null;
        [SerializeField] private Transform _AudioPlayer = null;
        public GraphicsPool GraphicsPool => _GraphicsPool;
        public ConfigContainer ConfigContainer => _ConfigContainer;
        public GameSaveContainer GameSaveContainer;

        public ServicesContainer ServicesContainer;

        protected bool _IsPaused;
        private void Awake()
        {
            DontDestroyOnLoad(this);
            Init();
        }

        protected virtual void Init()
        {
            InitGameSave(new GameSaveContainer());
            ServicesContainer = new ServicesContainer();
            ServicesContainer.Init();
            
            AudioPlayer.Instance.Init(GameSaveContainer.GetSave<SettingsGameSave>(), _AudioList, _AudioPlayer);
        }

        protected virtual void InitGameSave(GameSaveContainer container)
        {
            container.SetSave(new SettingsGameSave());
            
            GameSaveContainer = container;
            GameSaveContainer.LoadSaves();
            GameSaveContainer.Init();
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

        [SerializeField] protected Camera _Camera;
        public Camera Camera => _Camera;

        #endregion
    }
}
