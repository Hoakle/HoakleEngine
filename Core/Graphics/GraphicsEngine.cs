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
        }

        public override void Update()
        {
            
        }
        
        public AsyncOperation LoadScene(int index)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
            return asyncLoad;
        }
    }
}
