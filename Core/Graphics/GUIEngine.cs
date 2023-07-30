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
                T gui = InitGUI<T>(asyncOperation);
                onInstantiated?.Invoke(gui);
            };
        }

        public void CreateDataGUI<T, TData>(string key, TData data, Action<T> onInstantiated = null) where T : DataGUI<TData>
        {
            Addressables.InstantiateAsync(key).Completed += (asyncOperation) =>
            {
                T gui = InitDataGUI<T, TData>(asyncOperation, data);
                onInstantiated?.Invoke(gui);
            };
        }

        public void CreateGUIComponent<T>(string key, Transform parent, Action<T> onInstantiated = null) where T : GuiComponent
        {
            Addressables.InstantiateAsync(key, parent).Completed += (asyncOperation) =>
            {
                T gui = InitGUIComponent<T>(asyncOperation);
                onInstantiated?.Invoke(gui);
            };
        }
        
        public void CreateDataGUIComponent<T, TData>(string key, TData data, Transform parent, Action<T> onInstantiated = null) where T : DataGuiComponent<TData>
        {
            Addressables.InstantiateAsync(key, parent).Completed += (asyncOperation) =>
            {
                T gui = InitDataGUIComponent<T, TData>(asyncOperation, data);
                onInstantiated?.Invoke(gui);
            };
        }

        private T InitDataGUIComponent<T, TData>(AsyncOperationHandle<GameObject> asyncOperation, TData data) where T : DataGuiComponent<TData>
        {
            T dataGui = InitGUIComponent<T>(asyncOperation);
            if (dataGui is {})
            {
                dataGui.Data = data;
                return dataGui;
            }

            return null;
        }

        private T InitGUIComponent<T>(AsyncOperationHandle<GameObject> asyncOperation) where T : GuiComponent
        {
            if (asyncOperation.Result is { } gameObject)
            {
                if(gameObject.GetComponent<T>() is { } gui)
                {
                    gui.LinkEngine(this);
                    return gui;
                }
            }
            else if (asyncOperation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError(asyncOperation.OperationException);
            }
            
            return null;
        }

        private T InitDataGUI<T, TData>(AsyncOperationHandle<GameObject> asyncOperation, TData data) where T : DataGUI<TData>
        {
            T dataGui = InitGUI<T>(asyncOperation);
            if (dataGui is { })
            {
                dataGui.Data = data;
                return dataGui;
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

