using HoakleEngine.Core.Communication;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace HoakleEngine.Core.Graphics
{
    public abstract class GraphicsEngine : Engine
    {
        protected GUIEngine GuiEngine;
        
        public GraphicsEngine(GameRoot gameRoot) : base(gameRoot)
        {
            
        }

        public void Init(EventSystem eventSystem, Camera camera)
        {
            GuiEngine.Init(camera);
            SubscribesGenericEngineEvent();
        }

        public override void Update()
        {
            GuiEngine.Update();
        }

        private void SubscribesGenericEngineEvent()
        {
            EventBus.Instance.Subscribe<LoadSceneEvent>(LoadScene);
            EventBus.Instance.Subscribe<CreateGraphicalRepresentationEvent>(CreateGraphicalRepresentation);
        }
        private void LoadScene(LoadSceneEvent loadSceneEvent)
        {
            SceneManager.LoadSceneAsync(loadSceneEvent.SceneIndex);
        }

        private void CreateGraphicalRepresentation(CreateGraphicalRepresentationEvent creationEvent)
        {
            Addressables.InstantiateAsync(creationEvent.Key, creationEvent.Parent).Completed += (handle) => creationEvent.OnInstanciated?.Invoke(handle.Result);
        }
    }
}
