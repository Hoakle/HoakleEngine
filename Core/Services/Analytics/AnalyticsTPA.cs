using System;
using Firebase;
using Firebase.Analytics;
using HoakleEngine.Core.Services;

namespace HoakleEngine
{
    public class AnalyticsTPA : IAnalyticsTPA
    {
        private FirebaseApp _FirebaseApp;
        public Action<ActorError> OnError { get; set; }
        public void Init()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available) {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    _FirebaseApp = Firebase.FirebaseApp.DefaultInstance;

                    // Set a flag here to indicate whether Firebase is ready to use by your app.
                } else {
                    OnError?.Invoke(new AnalyticsError(AnalyticsErrorType.InitializationError, (int) dependencyStatus, $"Could not resolve all Firebase dependencies: {dependencyStatus}"));
                }
            });
        }

        public void SendEvent(string eventName)
        {
            FirebaseAnalytics.LogEvent(eventName);
        }

        public void SendEvent(string eventName, string paramName, int value)
        {
            FirebaseAnalytics.LogEvent(eventName, paramName, value);
        }
        
        public void SendEvent(string eventName, string paramName, float value)
        {
            FirebaseAnalytics.LogEvent(eventName, paramName, value);
        }
        
        public void SendEvent(string eventName, string paramName, string value)
        {
            FirebaseAnalytics.LogEvent(eventName, paramName, value);
        }
    }
}
