using System;
using System.Text;
using HoakleEngine.Core.Services.PlayServices;
using UnityEngine;

namespace HoakleEngine.Core.Services.AdsServices
{
    public class AdsThirdPartyService : ThirdPartyService
    {
        private AdsThirdPartyActor _Actor;

        public Action<string> OnLoaded
        {
            get => _Actor.OnLoaded;
            set => _Actor.OnLoaded = value;
        }

        public Action<string, bool> OnShowComplete 
        {
            get => _Actor.OnShowComplete;
            set => _Actor.OnShowComplete = value;
        }
        public override void Init()
        {
            _Actor = new AdsTPA();

            _Actor.OnError += OnError;
            _Actor.Init();
        }

        public void Load(string rewardedAd)
        {
            _Actor.Load(rewardedAd);
        }

        public void Show(string rewardedAd)
        {
            _Actor.Show(rewardedAd);
        }
        
#region Error handling
        private void OnError(ActorError error)
        {
            StringBuilder str = new StringBuilder();
            str.Append("Ads Services Error: " + ((AdsError) error).Type);
            str.AppendLine("    - Code: " + error.Code);
            str.AppendLine("    - Message: " + error.Message);
                    
            Debug.LogError(str.ToString());
        }
        
#endregion

    }
    
    public class AdsError : ActorError
    {
        public AdsErrorType Type;

        public AdsError(AdsErrorType type, int code, string message)
        {
            Type = type;
            Code = code;
            Message = message;
        }
    }

    public enum AdsErrorType
    {
        InitializationError = 0,
        AdsLoadError,
        AdsShowError
    }
}
