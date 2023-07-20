using HoakleEngine.Core.Communication;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HoakleEngine.Core.Graphics
{
    public abstract class GUIEngine : Engine
    {
        private Camera Camera;
        
        public GUIEngine(GameRoot gameRoot) : base(gameRoot)
        {
        
        }

        public void Init(Camera camera)
        {
            Camera = camera;
            SubscribesGenericEngineEvent();
        }

        private void SubscribesGenericEngineEvent()
        {
            EventBus.Instance.Subscribe<GUICreationEvent>(CreateGUI);
        }
        
        public void CreateGUI(GUICreationEvent guiCreationEvent)
        {
            Addressables.InstantiateAsync(guiCreationEvent.GUIName).Completed += InitGUI;
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

