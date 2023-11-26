using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Market
{
    public class MarketManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private ItemDataContainer itemDataContainer;

        private int _moneys;

        public void AddMoney(int rewardMoneys)
        {
            _moneys = itemDataContainer.money;
            _moneys += rewardMoneys;
            itemDataContainer.money = _moneys;
            price.text = $"▲{_moneys}";
        }
    
    }
}
