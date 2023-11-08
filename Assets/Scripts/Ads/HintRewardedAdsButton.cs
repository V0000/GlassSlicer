using System;
using System.Collections;
using Game;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Ads
{
    //Кнопка получения подсказок за просмотр рекламы
    public class HintRewardedAdsButton : RewardedAdsButton
    {
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private int rewardedHints;
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