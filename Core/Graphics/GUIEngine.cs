using HoakleEngine.Core.Communication;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HoakleEngine.Core.Graphics
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
            SubscribesGenericEngineEvent();
        }

        private void SubscribesGenericEngineEvent()
        {
            _EventBus.Subscribe<GUICreationEvent>(CreateGUI);
            _EventBus.Subscribe<DataGUICreationEvent>(CreateGUI);
        }
        
        public void CreateGUI(GUICreationEvent guiCreationEvent)
        {
            Addressables.InstantiateAsync(guiCreationEvent.GUIName).Completed += InitGUI;
        }
        
        public void CreateGUI(DataGUICreationEvent guiCreationEvent)
        {
            Addressables.InstantiateAsync(guiCreationEvent.GUIName).Completed += (prefab) =>
            {
                InitGUI(prefab);
                if (prefab.Result is { } gameObject)
                {
                    if(gameObject.GetComponent<ObjectRepresentation>() is { } objectRepresentation)
                        objectRepresentation.SetData(guiCreationEvent.Data);
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
                    gui.LinkEngine(this, _EventBus);
                }
            }
        }
    }
}

