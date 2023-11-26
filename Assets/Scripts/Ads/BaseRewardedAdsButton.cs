using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Ads
{
    // Базовый класс рекламы при нажатии на кнопку
    public class BaseRewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        #region Private variables
        
        [SerializeField] Button _showAdButton;
        [SerializeField] string _androidAdUnitId = "Rewarded_Android";
        [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
        string _adUnitId = null; // This will remain null for unsupported platforms

        #endregion
        void Awake()
        {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#endif

            // Дизейблим кнопку пока реклама не будет готова к показу
            _showAdButton.interactable = false;
        }

        private void Start()
        {
            StartCoroutine(LoadRevardedAds());
        }

        private IEnumerator LoadRevardedAds()
        {
            yield return new WaitForSeconds(1f);
            LoadAd();
        }

        #region Public methods
        
        public void LoadAd()
        {
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Ad Loaded: " + adUnitId);

            if (adUnitId.Equals(_adUnitId))
            {
                // Configure the button to call the ShowAd() method when clicked:
                _showAdButton.onClick.AddListener(ShowAd);
                // Enable the button for users to click:
                _showAdButton.interactable = true;
            }
        }

        public void ShowAd()
        {
            Debug.Log("Unity Ads Button is pressed");
            // Disable the button:
            _showAdButton.interactable = false;
            // Then show the ad:
            Advertisement.Show(_adUnitId, this);
        }

        public virtual void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                Debug.Log("Unity Ads Rewarded Ad Completed");
                // Grant a reward.
            }

            LoadAd();
        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        }

        public void OnUnityAdsShowStart(string adUnitId)
        {
        }

        public void OnUnityAdsShowClick(string adUnitId)
        {
        }
        
        #endregion

        void OnDestroy()
        {
            // Clean up the button listeners:
            _showAdButton.onClick.RemoveAllListeners();
        }
    }
}