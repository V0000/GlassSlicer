using TMPro;
using UnityEngine;

namespace Market
{
    public class MarketManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private LotDataContainer lotDataContainer;

        private int _moneys;

        public void AddMoney(int rewardMoneys)
        {
            _moneys = lotDataContainer.money;
            _moneys += rewardMoneys;
            lotDataContainer.money = _moneys;
            price.text = $"▲{_moneys}";
        }
    
    }
}
