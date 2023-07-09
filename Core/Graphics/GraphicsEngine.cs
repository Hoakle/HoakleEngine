using HoakleEngine.Core.Communication;
using HoakleEngine.Graphics;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace HoakleEngine.Core.Graphics
{
    public abstract class GraphicsEngine : Engine
    {
        private readonly GUIEngine GuiEngine;
        
        public GraphicsEngine(EventBus eventBus) : base(eventBus)
        {
            
        }

        public void Init(EventSystem eventSystem)
        {
            AddressableAssetSettings.BuildPlayerContent();
            base.Init();
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
