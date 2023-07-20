using System;
using System.Collections;
using System.Collections.Generic;
using HoakleEngine.Core.Graphics;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HoakleEngine.Core.Communication
{
    public class GUICreationEvent
    {
        public string GUIName;

        public GUICreationEvent(string name)
        {
            GUIName = name;
        }
    }

    public class LoadSceneEvent
    {
        public int SceneIndex;

        public LoadSceneEvent(int index)
        {
            SceneIndex = index;
        }
    }

    public class CreateGraphicalRepresentationEvent
    {
        public string Key;
        public Transform Parent;
        public Action<GameObject> OnInstanciated;

        public CreateGraphicalRepresentationEvent(string key, Transform parent = null, Action<GameObject> action = null)
        {
            Key = key;
            Parent = parent;
            OnInstanciated = action;
        }
    }
}
