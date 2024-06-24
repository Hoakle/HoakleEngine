using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace HoakleEngine.Core.Graphics
{
    public interface IGraphicsEngine : IEngine
    {
        public IGUIEngine GuiEngine { get; }
        public CameraControl CameraControl { get; }
        public Action OnCameraControlChange { get; set; }
        public AsyncOperation LoadSceneAsync(int sceneIndex);
    }
    public abstract class GraphicsEngine : Engine, IGraphicsEngine
    {
        public IGUIEngine GuiEngine => _GuiEngine;

        public Action OnCameraControlChange
        {
            get => _OnCameraControlChange;
            set => _OnCameraControlChange = value;
        }

        private DiContainer _DiContainer;
        protected IGUIEngine _GuiEngine;
        private CameraControl _CameraControl;
        private Transform _GameContainer;
        private Action _OnCameraControlChange;

        public CameraControl CameraControl
        {
            get => _CameraControl;
            set
            {
                _CameraControl = value;
                _OnCameraControlChange?.Invoke();
            }
        }

        [Inject]
        public void Inject(
            DiContainer container,
            IGUIEngine guiEngine,
            [Inject (Id = GameRootIdentifier.GameContainer)] Transform gameContainer)
        {
            _DiContainer = container;
            _GuiEngine = guiEngine;
            _GameContainer = gameContainer;
        }

        public override void Init()
        {
            _GuiEngine.Init();
            _GuiEngine.LinkEngine(this);
        }

        public override void Update(bool isPaused)
        {
            _GuiEngine.Update(isPaused);
            base.Update(isPaused);
        }
        
        public AsyncOperation LoadSceneAsync(int sceneIndex)
        {
            return SceneManager.LoadSceneAsync(sceneIndex);
        }

        public void CreateGraphicalRepresentation<T, TData>(string key, TData data, Transform parent = null, Action<T> OnInstanciated = null) where T : GraphicalObjectRepresentation<TData>
        {
            var gameObject = _GameRoot.GraphicsPool.GetGraphics<T>();
            if (gameObject != null)
            {
                T gor = InitGraphicalRepresentation<T, TData>(gameObject, data, parent);
                gor.OnReady();
                OnInstanciated?.Invoke(gor);
            }
            else
            {
                Addressables.InstantiateAsync(key, parent ? parent : _GameContainer).Completed += (asyncOperation) =>
                {
                    if (asyncOperation.Result is { } gameObject)
                    {
                        T gor = InitGraphicalRepresentation<T, TData>(gameObject, data, parent);
                        _DiContainer.InjectGameObject(gor.gameObject);
                        gor.OnReady();
                        OnInstanciated?.Invoke(gor);
                    }
                    else if (asyncOperation.Status == AsyncOperationStatus.Failed)
                    {
                        Debug.LogError(asyncOperation.OperationException);
                    }
                };
            }
            
        }
        
        public void CreateGraphicalRepresentation<T>(string key, Transform parent = null, Action<T> OnInstanciated = null) where T : GraphicalObjectRepresentation
        {
            var gameObject = _GameRoot.GraphicsPool.GetGraphics<T>();
            if (gameObject != null)
            {
                T gor = InitGraphicalRepresentation<T>(gameObject, parent);
                gor.OnReady();
                OnInstanciated?.Invoke(gor);
            }
            else
            {
                Addressables.InstantiateAsync(key, parent ? parent : _GameContainer).Completed += (asyncOperation) =>
                {
                    if (asyncOperation.Result is { } gameObject)
                    {
                        T gor = InitGraphicalRepresentation<T>(gameObject, parent);
                        _DiContainer.InjectGameObject(gor.gameObject);
                        gor.OnReady();
                        OnInstanciated?.Invoke(gor);
                    }
                    else if (asyncOperation.Status == AsyncOperationStatus.Failed)
                    {
                        Debug.LogError(asyncOperation.OperationException);
                    }
                };
            }
            
        }

        private T InitGraphicalRepresentation<T, TData>(GameObject gameObject, TData data, Transform parent = null) where T : GraphicalObjectRepresentation<TData>
        {
            if(gameObject.GetComponent<T>() is { } objectRepresentation)
            {
                objectRepresentation.Data = data;
                objectRepresentation.LinkEngine(this);
                objectRepresentation.gameObject.transform.parent = parent ? parent : _GameContainer;
                objectRepresentation.gameObject.SetActive(true);
                return objectRepresentation;
            }
            
            return null;
        }
        
        private T InitGraphicalRepresentation<T>(GameObject gameObject, Transform parent = null) where T : GraphicalObjectRepresentation
        {
            if(gameObject.GetComponent<T>() is { } objectRepresentation)
            {
                objectRepresentation.LinkEngine(this);
                objectRepresentation.gameObject.transform.parent = parent ? parent : _GameContainer;
                objectRepresentation.gameObject.SetActive(true);
                return objectRepresentation;
            }
            
            return null;
        }

        public void Dispose(Type type, GameObject gameObject)
        {
            if (!_GameRoot.GraphicsPool.AddToPool(type, gameObject))
                Object.Destroy(gameObject);
        }
    }
}
