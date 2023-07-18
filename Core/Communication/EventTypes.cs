using System.Collections;
using System.Collections.Generic;
using HoakleEngine.Core.Graphics;
using UnityEngine;

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
}
