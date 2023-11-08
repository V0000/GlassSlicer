using System;
using System.Collections;
using Market;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Ads
{
    //Кнопка получения игровой валюты за просмотр рекламы
    public class MoneyRewardedAdsButton : RewardedAdsButton
    {
        [SerializeField] MarketManager marketManager;
        [SerializeField] int rewardedMoney;
        
        
        public override void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                marketManager.AddMoney(rewardedMoney);
            }
            LoadAd();
        }
    }
}