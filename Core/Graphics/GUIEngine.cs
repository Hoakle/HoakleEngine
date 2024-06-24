using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HoakleEngine.Core.Game;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace HoakleEngine.Core.Graphics
{
    public interface IGUIEngine : IEngine
    {
        public Camera Camera { get; }
        public void CreateGUI<T>(string key, Action<T> onInstantiated = null) where T : GraphicalUserInterface;
        public void CreateDataGUI<T, TData>(string key, TData data, Action<T> onInstantiated = null)
            where T : DataGUI<TData>;
        public void CreateGUIComponent<T>(string key, Transform parent, Action<T> onInstantiated = null)
            where T : GuiComponent;
        public void CreateDataGUIComponent<T, TData>(string key, TData data, Transform parent,
            Action<T> onInstantiated = null) where T : DataGuiComponent<TData>;
        public T InitDataGUIComponent<T, TData>(DataGuiComponent<TData> component, TData data)
            where T : DataGuiComponent<TData>;
        public T InitGUIComponent<T>(GuiComponent gameObject) where T : GuiComponent;
        public void Dispose(GraphicalUserInterface graphicalUserInterface);
    }
    
    public abstract class GUIEngine : Engine, IGUIEngine
    {
        private Camera _Camera;
        public Camera Camera => _Camera;
        private List<Canvas> _SortedCanvas;
        private Transform _GameContainer;
        private DiContainer _DiContainer;

        [Inject]
        public void Inject(DiContainer container)
        {
            _DiContainer = container;
        }
        
        public GUIEngine()
        {
            _SortedCanvas = new List<Canvas>();
        }

        public void CreateGUI<T>(string key, Action<T> onInstantiated = null) where T : GraphicalUserInterface
        {
            var asyncOperation = Addressables.InstantiateAsync(key, _GameContainer);
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
            Addressables.InstantiateAsync(key, _GameContainer).Completed += (asyncOperation) =>
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
                    _DiContainer.InjectGameObject(gui.gameObject);
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
                _DiContainer.InjectGameObject(gui.gameObject);
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
                _DiContainer.InjectGameObject(gui.gameObject);
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

