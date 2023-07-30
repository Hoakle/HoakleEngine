using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace HoakleEngine.Core.Graphics
{
    public abstract class GraphicsEngine : Engine
    {
        public GUIEngine GuiEngine;
        public CameraControl CameraControl;
        public GraphicsEngine(GameRoot gameRoot) : base(gameRoot)
        {
            
        }

        public override void Init() { }
        public virtual void Init(Camera camera)
        {
            GuiEngine.Init(camera);
            GuiEngine.LinkEngine(this);
        }

        public override void Update()
        {
            GuiEngine.Update();
            base.Update();
        }
        
        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadSceneAsync(sceneIndex);
        }

        public void CreateGraphicalRepresentation<T, TData>(string key, TData data, Transform parent = null, Action<T> OnInstanciated = null) where T : GraphicalObjectRepresentation<TData>
        {
            Addressables.InstantiateAsync(key, parent).Completed += (handle) =>
            {
                T gor = InitGraphicalRepresentation<T, TData>(handle, data, parent);
                OnInstanciated?.Invoke(gor);
            };
        }

        private T InitGraphicalRepresentation<T, TData>(AsyncOperationHandle<GameObject> asyncOperation, TData data, Transform parent = null) where T : GraphicalObjectRepresentation<TData>
        {
            if (asyncOperation.Result is { } gameObject)
            {
                if(gameObject.GetComponent<T>() is { } objectRepresentation)
                {
                    objectRepresentation.Data = data;
                    objectRepresentation.LinkEngine(this);
                    objectRepresentation.Parent = parent;
                    return objectRepresentation;
                }
            }
            else if (asyncOperation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError(asyncOperation.OperationException);
            }
            
            return null;
        }
    }
}
