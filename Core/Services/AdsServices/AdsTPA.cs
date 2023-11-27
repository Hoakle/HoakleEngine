using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace HoakleEngine.Core.Services.AdsServices
{
    public class AdsTPA : AdsThirdPartyActor, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private string _AndroidGameId = "5461893";
        private string _IOSGameId = "5461892";
        private bool _testMode = false;
        private string _gameId;
        public Action<ActorError> OnError { get; set; }
        public Action<string> OnLoaded { get; set; }
        public Action<string, bool>  OnShowComplete { get; set; }

        public void Init()
        {
            InitializeAds();
        }

        public void InitializeAds()
        {
    #if UNITY_IOS
                _gameId = _IOSGameId;
    #elif UNITY_ANDROID
            _gameId = _AndroidGameId;
    #elif UNITY_EDITOR
                _gameId = _AndroidGameId; //Only for testing the functionality in the Editor
    #endif
            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(_gameId, _testMode, this);
            }
        }

        public void Load(string rewardedAd)
        {
            Advertisement.Load(rewardedAd, this);
        }

        public void Show(string rewardedAd)
        {
            Advertisement.Show(rewardedAd, this);
        }

        public void OnInitializationComplete()
        {
            Debug.Log("<color=purple> Ads Services </color> - Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            OnError?.Invoke(new AdsError(AdsErrorType.InitializationError, (int) error, $"Unity Ads Initialization Failed: {error.ToString()} - {message}"));
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"<color=purple> Ads Services </color> - Unity Ads loaded {placementId}.");
            OnLoaded?.Invoke(placementId);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            OnError?.Invoke(new AdsError(AdsErrorType.AdsLoadError, (int) error, $"Unity Ads load Failed: PlacementId: {placementId} - {error.ToString()} - {message}"));
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            OnError?.Invoke(new AdsError(AdsErrorType.AdsShowError, (int) error, $"Unity Ads show Failed: PlacementId: {placementId} - {error.ToString()} - {message}"));
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            throw new NotImplementedException();
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            throw new NotImplementedException();
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Debug.Log($"<color=purple> Ads Services </color> - Unity Ads show completed - {placementId}.");
            OnShowComplete?.Invoke(placementId, showCompletionState == UnityAdsShowCompletionState.COMPLETED);
        }
    }
}
