using UnityEngine;
using UnityEngine.Advertisements;

namespace Ads
{
    //Межсценовая реклама (решено пока не реализовывать)
    public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        #region Private variables
        
        [SerializeField] private string _androidAdUnityId = "Interstitial_Android";
        [SerializeField] private string _iOsAdUnityId = "Interstitial_iOS";
        private string _adUnityId;
        
        #endregion
        
        public static InterstitialAd StaticAd;
        void Awake()
        {
            StaticAd = this;
            _adUnityId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOsAdUnityId
                : _androidAdUnityId;
            LoadAd();
        }

        public void LoadAd()
        {
            Advertisement.Load(_adUnityId, this);
        }
    
        #region Public methods

        
        public void ShowAd()
        {
            Advertisement.Show(_adUnityId, this);
        }
    
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            LoadAd();
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
        }

        public void OnUnityAdsShowStart(string placementId)
        {
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        #endregion

    }
}
