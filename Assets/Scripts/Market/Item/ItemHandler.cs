using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Market
{
    public class ItemHandler : MonoBehaviour
    {
        #region Private variables

        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private GameObject lockObject;
        [SerializeField] private Image backGroundImage;

        private ItemDataContainer _itemsData;
        private UIMoneyHandler _uiMoneyHandler;
        private Button _itemButton;
        private Item _currentItem;
        private int _id;

        #endregion

  

        private void Start()
        {
            Debug.Log($"itemDataContainer == null = {_itemsData == null}");
            _itemButton = GetComponent<Button>();

            if (_itemButton != null)
            {
                _itemButton.onClick.AddListener(OnClick);
            }
            else
            {
                Debug.LogError("Button component not found.");
            }
        }

        public void SetItemValues(ItemDataContainer itemsData, Item currentItem, int id, UIMoneyHandler uiMoneyHandler)
        {
            _id = id;
            _itemsData = itemsData;
            _currentItem = currentItem;
            _uiMoneyHandler = uiMoneyHandler;
            price.text = $"▲{currentItem.price}";
            price.gameObject.SetActive(currentItem.isLocked);
            lockObject.gameObject.SetActive(currentItem.isLocked);

            if (backGroundImage != null)
            {
                backGroundImage.sprite = currentItem.image;
            }
            else
            {
                Debug.Log("Image component not found.");
            }
        }

        void OnClick()
        {
            bool bgIsChanged = false;

            if (_currentItem.isLocked) //заблокирован
            {
                if (_currentItem.price <= _itemsData.money) //хватает денег
                {
                    _itemsData.money -= _currentItem.price;
                    _itemsData.items[_id].isLocked = false;
                    _currentItem.isLocked = false;

                    price.gameObject.SetActive(false);
                    lockObject.gameObject.SetActive(false);
                    _uiMoneyHandler.SetMoneyText(_itemsData.money);

                    _itemsData.activeBackground = _currentItem.image;
                    _itemsData.activeID = _id;
                    bgIsChanged = true;
                }
                else //денег не достаточно
                {
                    _uiMoneyHandler.PlayShakeAnimation();
                }
            }
            else //не заблокирован
            {
                int defaultID = 0;
                
                if (_id == defaultID) //первый бг дефолтный, без спрайта
                {
                    _itemsData.activeBackground = null;
                }
                else
                {
                    _itemsData.activeBackground = _currentItem.image;
                }

                _itemsData.activeID = _id;
                bgIsChanged = true;
            }


            if (bgIsChanged)
            {
                SceneManager.LoadScene("Level");
                Debug.Log("BG is choosed");
            }
        }
    }
}