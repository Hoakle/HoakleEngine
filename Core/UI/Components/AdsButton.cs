using System;
using HoakleEngine.Core.Config.Ads;
using HoakleEngine.Core.Graphics;
using HoakleEngine.Core.Localization;
using HoakleEngine.Core.Services.AdsServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HoakleEngine.Core.UI.Components
{
    public class AdsButton : DataGuiComponent<AdsConfigData>
    {
        [SerializeField] private Button _Button = null;
        [SerializeField] private LocalizedText _Label = null;
        private string _AdUnitId = "Rewarded_Android";

        public Action OnClaimReward;
        private AdsThirdPartyService _AdsTP;

        [Inject]
        public void Inject(AdsThirdPartyService adsThirdPartyService)
        {
            _AdsTP = adsThirdPartyService;
        }
        
        public override void OnReady()
        {
            _AdsTP.OnLoaded += AdLoaded;
            _AdsTP.OnShowComplete += AdShowComplete;
            
            _Button.interactable = false;
            SetText();
            LoadAd();
            base.OnReady();
        }

        private void SetText()
        {
            if (Data.Type == AdsType.SIMPLE)
                _Label.SetKey("Ads/Continue");
            else
            {
                _Label.SetKey("x" + Data.Value);
            }
        }
        private void LoadAd()
        {
            _AdUnitId = Data.Id;
            _AdsTP.Load(_AdUnitId);
        }
        
        private void AdLoaded(string adUnitId)
        {
            if (adUnitId.Equals(_AdUnitId))
            {
                // Configure the button to call the ShowAd() method when clicked:
                _Button.onClick.AddListener(ShowAd);
                // Enable the button for users to click:
                _Button.interactable = true;
            }
        }
        
        public void ShowAd()
        {
            // Disable the button:
            _Button.interactable = false;
            // Then show the ad:
            _AdsTP.Show(_AdUnitId);
        }

        private void AdShowComplete(string adUnit, bool completed)
        {
            if (adUnit.Equals(_AdUnitId) && completed)
            {
                OnClaimReward?.Invoke();
            }
        }
    }
}
