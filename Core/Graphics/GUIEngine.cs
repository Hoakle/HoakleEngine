using System;
using System.Collections;
using System.Collections.Generic;
using HoakleEngine.Core.Game;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HoakleEngine.Core.Graphics
{
    public abstract class GUIEngine : Engine
    {
        private Camera _Camera;
        public Camera Camera => _Camera;
        private List<Canvas> _SortedCanvas;
        public GUIEngine(GameRoot gameRoot) : base(gameRoot)
        {
            _SortedCanvas = new List<Canvas>();
        }

        public void Init(Camera camera)
        {
            _Camera = camera;
        }

        public void CreateGUI<T>(string key, Action<T> onInstantiated = null) where T : GraphicalUserInterface
        {
            var asyncOperation = Addressables.InstantiateAsync(key);
            asyncOperation.Completed += (asyncOperation) =>
            {
                if (asyncOperation.Result is { } gameObject)
                {
                    T gui = InitGUI<T>(gameObject);
                    gui.OnReady();
                    onInstantiated?.Invoke(gui);
                }
                else if (asyncOperation.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError(asyncOperation.OperationException);
                }
                else
                {
                    Debug.LogError("Error: Addressables instantiation failed: Status = " + asyncOperation.Status);
                }
            };
        }
        
        public void CreateDataGUI<T, TData>(string key, TData data, Action<T> onInstantiated = null) where T : DataGUI<TData>
        {
            Addressables.InstantiateAsync(key).Completed += (asyncOperation) =>
            {
                if (asyncOperation.Result is { } gameObject)
                {
                    T gui = InitDataGUI<T, TData>(gameObject, data);
                    onInstantiated?.Invoke(gui);
                }
                else if (asyncOperation.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError(asyncOperation.OperationException);
                }
            };
        }

        public void CreateGUIComponent<T>(string key, Transform parent, Action<T> onInstantiated = null) where T : GuiComponent
        {
            Addressables.InstantiateAsync(key, parent).Completed += (asyncOperation) =>
            {
                if (asyncOperation.Result is { } gameObject)
                {
                    T gui = InitGUIComponent<T>(gameObject);
                    gui.OnReady();
                    onInstantiated?.Invoke(gui);
                }
                else if (asyncOperation.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError(asyncOperation.OperationException);
                }
            };
        }
        
        public void CreateDataGUIComponent<T, TData>(string key, TData data, Transform parent, Action<T> onInstantiated = null) where T : DataGuiComponent<TData>
        {
            Addressables.InstantiateAsync(key, parent).Completed += (asyncOperation) =>
            {
                if (asyncOperation.Result is { } gameObject)
                {
                    T gui = InitDataGUIComponent<T, TData>(gameObject, data);
                    onInstantiated?.Invoke(gui);
                }
                else if (asyncOperation.Status == AsyncOperationStatus.Failed)
                {
                    Debug.LogError(asyncOperation.OperationException);
                }
            };
        }

        private T InitDataGUIComponent<T, TData>(GameObject gameObject, TData data) where T : DataGuiComponent<TData>
        {
            T dataGui = InitGUIComponent<T>(gameObject);
            if (dataGui is {})
            {
                dataGui.Data = data;
                dataGui.OnReady();
                return dataGui;
            }

            return null;
        }
        
            
        public T InitDataGUIComponent<T, TData>(DataGuiComponent<TData> component, TData data) where T : DataGuiComponent<TData>
        {
            T dataGui = InitGUIComponent<T>(component.gameObject);
            if (dataGui is {})
            {
                dataGui.Data = data;
                dataGui.OnReady();
                return dataGui;
            }

            return null;
        }

        private T InitGUIComponent<T>(GameObject gameObject) where T : GuiComponent
        {
                if(gameObject.GetComponent<T>() is { } gui)
                {
                    gui.LinkEngine(this);
                    return gui;
                }
                else
                {
                    Debug.LogError(gameObject.name + " is not " + typeof(T));
                }

                return null;
        }
        
        public T InitGUIComponent<T>(GuiComponent gameObject) where T : GuiComponent
        {
            if(gameObject.GetComponent<T>() is { } gui)
            {
                gui.LinkEngine(this);
                gameObject.OnReady();
                return gui;
            }
            else
            {
                Debug.LogError(gameObject.name + " is not " + typeof(T));
            }

            return null;
        }

        private T InitDataGUI<T, TData>(GameObject gameObject, TData data) where T : DataGUI<TData>
        {
            T dataGui = InitGUI<T>(gameObject);
            if (dataGui is { })
            {
                dataGui.Data = data;
                dataGui.OnReady();
                return dataGui;
            }

            return null;
        }

        private T InitGUI<T>(GameObject gameObject) where T : GraphicalUserInterface
        {
            if(gameObject.GetComponent<T>() is { } gui)
            {
                gui.LinkEngine(this);
                gui.Canvas.worldCamera = Camera;
                gui.Canvas.planeDistance = 1f;
                _SortedCanvas.Add(gui.Canvas);
                ApplySorting();
                return gui;
            }
            else
            {
                Debug.LogError(gameObject.name + " is not " + typeof(T));
            }
            
            return null;
        }

        private void ApplySorting()
        {
            for(int i = 0; i < _SortedCanvas.Count; i++)
            {
                _SortedCanvas[i].sortingOrder = i;
            }
        }
        public void Dispose(GraphicalUserInterface graphicalUserInterface)
        {
            _SortedCanvas.Remove(graphicalUserInterface.Canvas);
            ApplySorting();
        }
    }
}

