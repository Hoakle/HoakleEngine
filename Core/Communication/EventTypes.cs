using System.Collections;
using System.Collections.Generic;
using HoakleEngine.Core.Graphics;
using UnityEngine;

namespace HoakleEngine.Core.Communication
{
    public class EventTypes
    {
        
    }

    public class GUICreationEvent
    {
        public string GUIName;

        public GUICreationEvent(string name)
        {
            GUIName = name;
        }
    }

    public class DataGUICreationEvent : GUICreationEvent
    {
        public Object Data;
        
        public DataGUICreationEvent(string name, Object data) : base(name)
        {
            Data = data;
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
}
