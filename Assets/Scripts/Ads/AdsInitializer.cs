using UnityEngine;
using UnityEngine.Advertisements;

namespace Ads
{
    // Инициализация рекламы
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {
        #region Private variables
        
        [SerializeField] private string _androidGameId = "5446719";
        [SerializeField] private string _iOSGameId = "5446718";
        [SerializeField] private bool _testMode = true;
        private string _gameId;
        
        #endregion
        
        void Awake()
        {
            InitializeAds();
        }
        
        #region Public methods
        
        public void InitializeAds()
        {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
            _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(_gameId, _testMode, this);
            }
        }


        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
        
        #endregion
    }
}