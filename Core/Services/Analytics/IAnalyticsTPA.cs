using System;
using System.Collections;
using System.Collections.Generic;
using HoakleEngine.Core.Services;
using UnityEngine;

namespace HoakleEngine
{
    public interface IAnalyticsTPA : ThirdPartyActor
    {
        public void SendEvent(string eventName);
        public void SendEvent(string eventName, string paramName, int value);
        public void SendEvent(string eventName, string paramName, float value);
        public void SendEvent(string eventName, string paramName, string value);
    }
}
