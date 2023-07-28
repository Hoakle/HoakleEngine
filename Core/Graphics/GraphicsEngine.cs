using System;
using TreeEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace HoakleEngine.Core.Graphics
{
    public abstract class GraphicsEngine : Engine
    {
        public GUIEngine GuiEngine;
        
        public GraphicsEngine(GameRoot gameRoot) : base(gameRoot)
        {
            
        }

        public override void Init() { }
        public virtual void Init(EventSystem eventSystem, Camera camera)
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

        }
        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadSceneAsync(sceneIndex);
        }

        public void CreateGraphicalRepresentation(string key, Transform parent, Action<GameObject> OnInstanciated)
        {
            Addressables.InstantiateAsync(key, parent).Completed += (handle) => OnInstanciated?.Invoke(handle.Result);
        }
    }
}
