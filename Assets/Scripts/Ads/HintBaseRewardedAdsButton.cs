using Game;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace Ads
{
    //Кнопка получения подсказок за просмотр рекламы
    public class HintBaseRewardedAdsButton : BaseRewardedAdsButton
    {
        
        private GameManager _gameManager;
        [SerializeField] private int rewardedHints = 2;
        
        
        [Inject]
        private void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
        }
        
        
        public override void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                _gameManager.AddHint(rewardedHints);
            }
            LoadAd();
        }
    }
}