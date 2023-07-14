using HoakleEngine.Core.Communication;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace HoakleEngine.Core.Graphics
{
    public abstract class GraphicsEngine : Engine
    {
        protected GUIEngine GuiEngine;
        
        public GraphicsEngine(EventBus eventBus) : base(eventBus)
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
            _EventBus.Subscribe<LoadSceneEvent>(LoadScene);
        }
        public void LoadScene(LoadSceneEvent loadSceneEvent)
        {
            SceneManager.LoadSceneAsync(loadSceneEvent.SceneIndex);
        }
    }
}
