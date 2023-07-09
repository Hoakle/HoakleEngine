using HoakleEngine.Core;
using HoakleEngine.Core.Communication;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HoakleEngine.Graphics
{
    public abstract class GUIEngine : Engine
    {
        private Camera Camera;
        
        public GUIEngine(EventBus eventBus) : base(eventBus)
        {
        
        }

        public void Init(Camera camera)
        {
            Camera = camera;
        }

        public void CreateGUI(string key)
        {
            Addressables.InstantiateAsync(key).Completed += InitGUI;
        }
        
        public void CreateGUI<T, TData>(string key, TData data, Transform parent = null) where T : ObjectRepresentation<TData>
        {
            Addressables.InstantiateAsync(key).Completed += (prefab) =>
            {
                InitGUI(prefab);
                if (prefab.Result is { } gameObject)
                {
                    if(gameObject.GetComponent<T>() is { } objectRepresentation)
                        objectRepresentation.SetData(data);
                }
            };
        }

        private void InitGUI(AsyncOperationHandle<GameObject> prefab)
        {
            if (prefab.Result is { } gameObject)
            {
                if(gameObject.GetComponent<Canvas>() is { } canvas)
                {
                    canvas.worldCamera = Camera;
                    canvas.planeDistance = 0.5f;
                }
                
                if(gameObject.GetComponent<GraphicalUserInterface>() is { } gui)
                {
                    gui.LinkEngine(this);
                }
            }
        }
    }
}

