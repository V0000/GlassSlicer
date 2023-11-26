
using Market;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;
namespace Ads
{
    //Кнопка получения игровой валюты за просмотр рекламы
    public class MoneyBaseRewardedAdsButton : BaseRewardedAdsButton
    {
        private MarketManager _marketManager;
        [SerializeField] private int rewardedMoney = 15;
        
        [Inject]
        private void Initialize(MarketManager marketManager)
        {
            _marketManager = marketManager;
        }
        
        public override void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                _marketManager.AddMoney(rewardedMoney);
            }
            LoadAd();
        }
    }
}