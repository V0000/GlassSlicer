using Map;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Market
{
    public class MarketBuilder : MonoBehaviour
    {
        #region Private variables
        [SerializeField] private ItemHandler itemPrefab;
        [SerializeField] private ItemsSlideViewPanel itemSlideViewPanel;
        [SerializeField] private ScrollMover scrollMover;
        [SerializeField] private UIMoneyHandler uiMoneyHandler;
        
        private ItemDataContainer _itemsData;
        #endregion
        
        [Inject]
        public void Initialize(ItemDataContainer itemsData)
        {
            _itemsData = itemsData;
        }
        void Start()
        {
            int id = 0;
            foreach (Item lot in _itemsData.items)
            {
            
                GameObject instantiatedObject = Instantiate(itemPrefab.gameObject);
                instantiatedObject.transform.SetParent(itemSlideViewPanel.transform, false);

                ItemHandler instantiatedItem = instantiatedObject.GetComponent<ItemHandler>();
                instantiatedItem.SetItemValues(_itemsData, lot, id, uiMoneyHandler);
                instantiatedObject.SetActive(true);
                id++;

            }

            uiMoneyHandler.SetMoneyText(_itemsData.money);
            float scrollPosition = (float)_itemsData.activeID / (float)(_itemsData.items.Length-1);
            scrollMover.SetScrollValueY(scrollPosition);
        }


    }
}
