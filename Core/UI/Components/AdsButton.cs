using System;
using HoakleEngine.Core.Graphics;
using HoakleEngine.Core.Services.AdsServices;
using RetroRush.Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HoakleEngine.Core.UI.Components
{
    public class AdsButton : DataGuiComponent<AdsConfigData>
    {
        [SerializeField] private Button _Button = null;
        [SerializeField] private TextMeshProUGUI _Label = null;
        private string _AdUnitId = "Rewarded_Android";

        public Action OnClaimReward;
        private AdsThirdPartyService _AdsTP;
        public override void OnReady()
        {
            _AdsTP = _GuiEngine.ServicesContainer.GetService<AdsThirdPartyService>();
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
                _Label.text = "CONTINUE";
            else
            {
                _Label.text = "x" + Data.Value;
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
