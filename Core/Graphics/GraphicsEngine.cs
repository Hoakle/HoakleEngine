using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace HoakleEngine.Core.Graphics
{
    public abstract class GraphicsEngine : Engine
    {
        public GUIEngine GuiEngine;

        public Action OnCameraControlChange;
        private CameraControl _CameraControl;
        public CameraControl CameraControl
        {
            get => _CameraControl;
            set
            {
                _CameraControl = value;
                OnCameraControlChange?.Invoke();
            }
        }
        public GraphicsEngine(GameRoot gameRoot) : base(gameRoot)
        {
            
        }

        public override void Init() { }
        public virtual void Init(Camera camera)
        {
            GuiEngine.Init(camera);
            GuiEngine.LinkEngine(this);
        }

        public override void Update(bool isPaused)
        {
            GuiEngine.Update(isPaused);
            base.Update(isPaused);
        }
        
        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void CreateGraphicalRepresentation<T, TData>(string key, TData data, Transform parent = null, Action<T> OnInstanciated = null) where T : GraphicalObjectRepresentation<TData>
        {
            var gameObject = _GameRoot.GraphicsPool.GetGraphics<T>();
            if (gameObject != null)
            {
                T gor = InitGraphicalRepresentation<T, TData>(gameObject, data, parent);
                OnInstanciated?.Invoke(gor);
            }
            else
            {
                Addressables.InstantiateAsync(key, parent).Completed += (asyncOperation) =>
                {
                    if (asyncOperation.Result is { } gameObject)
                    {
                        T gor = InitGraphicalRepresentation<T, TData>(gameObject, data, parent);
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
                OnInstanciated?.Invoke(gor);
            }
            else
            {
                Addressables.InstantiateAsync(key, parent).Completed += (asyncOperation) =>
                {
                    if (asyncOperation.Result is { } gameObject)
                    {
                        T gor = InitGraphicalRepresentation<T>(gameObject, parent);
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
                objectRepresentation.gameObject.transform.parent = parent;
                objectRepresentation.gameObject.SetActive(true);
                objectRepresentation.OnReady();
                return objectRepresentation;
            }
            
            return null;
        }
        
        private T InitGraphicalRepresentation<T>(GameObject gameObject, Transform parent = null) where T : GraphicalObjectRepresentation
        {
            if(gameObject.GetComponent<T>() is { } objectRepresentation)
            {
                objectRepresentation.LinkEngine(this);
                objectRepresentation.gameObject.transform.parent = parent;
                objectRepresentation.gameObject.SetActive(true);
                objectRepresentation.OnReady();
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
