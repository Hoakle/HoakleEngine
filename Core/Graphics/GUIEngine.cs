using System;
using HoakleEngine.Core.Game;
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
        }

        public void CreateGUI<T>(string key, Action<T> onInstantiated = null) where T : GraphicalUserInterface
        {
            Addressables.InstantiateAsync(key).Completed += (asyncOperation) =>
            {
                onInstantiated?.Invoke(InitGUI<T>(asyncOperation));
            };
        }
        
        public void CreateGUI<T>(string key, Transform parent, Action<T> onInstantiated = null) where T : GraphicalUserInterface
        {
            Addressables.InstantiateAsync(key, parent).Completed += (asyncOperation) =>
            {
                onInstantiated?.Invoke(InitGUI<T>(asyncOperation));
            };
        }
        
        public void CreateDataGUI<T, TData>(string key, TData data, Transform parent, Action<T> onInstantiated = null) where T : DataGUI<TData>
        {
            Addressables.InstantiateAsync(key, parent).Completed += (asyncOperation) =>
            {
                onInstantiated?.Invoke(InitDataGUI<T, TData>(asyncOperation, data));
            };
        }
        
        public void CreateDataGUI<T, TData>(string key, TData data, Action<T> onInstantiated = null) where T : DataGUI<TData>
        {
            Addressables.InstantiateAsync(key).Completed += (asyncOperation) =>
            {
                onInstantiated?.Invoke(InitDataGUI<T, TData>(asyncOperation, data));
            };
        }

        private T InitDataGUI<T, TData>(AsyncOperationHandle<GameObject> asyncOperation, TData data) where T : DataGUI<TData>
        {
            InitGUI<T>(asyncOperation);
            if (asyncOperation.Result is { } gameObject)
            {
                if(gameObject.GetComponent<T>() is { } dataGui)
                {
                    dataGui.Data = data;
                    return dataGui;
                }
            }

            return null;
        }

        private T InitGUI<T>(AsyncOperationHandle<GameObject> asyncOperation) where T : GraphicalUserInterface
        {
            if (asyncOperation.Result is { } gameObject)
            {
                if(gameObject.GetComponent<T>() is { } gui)
                {
                    gui.LinkEngine(this);
                    gui.Canvas.worldCamera = Camera;
                    gui.Canvas.planeDistance = 0.5f;
                    return gui;
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

