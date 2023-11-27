using System.Collections;
using System.Collections.Generic;
using HoakleEngine.Core.Services;
using UnityEngine;

namespace HoakleEngine
{
    public class AnalyticsTP : ThirdPartyService
    {
        private IAnalyticsTPA _AnalyticsTPA;
        public override void Init()
        {
            _AnalyticsTPA = new AnalyticsTPA();
        }
        
        public void SendEvent(string eventName)
        {
            _AnalyticsTPA.SendEvent(eventName);
        }

        public void SendEvent(string eventName, string paramName, int value)
        {
            _AnalyticsTPA.SendEvent(eventName, paramName, value);
        }
        
        public void SendEvent(string eventName, string paramName, float value)
        {
            _AnalyticsTPA.SendEvent(eventName, paramName, value);
        }
        
        public void SendEvent(string eventName, string paramName, string value)
        {
            _AnalyticsTPA.SendEvent(eventName, paramName, value);
        }
    }

    public static class EventName
    {
        public static string LevelStarted = "StartLevel";
    }
    public class AnalyticsError : ActorError
    {
        public AnalyticsErrorType Type;
        public AnalyticsError(AnalyticsErrorType type, int code, string message)
        {
            Type = type;
            Code = code;
            Message = message;
        }
    }
    
    public enum AnalyticsErrorType
    {
        InitializationError = 0,
    }
}
