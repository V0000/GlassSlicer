using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Market
{
    public class MarketManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI price;
        private ItemDataContainer _itemsData;
        private int _moneys;
        
        public void Initialize(ItemDataContainer itemsData)
        {
            _itemsData = itemsData;
        }

        public void AddMoney(int rewardMoneys)
        {
            _moneys = _itemsData.money;
            _moneys += rewardMoneys;
            _itemsData.money = _moneys;
            price.text = $"▲{_moneys}";
        }
    
    }
}
