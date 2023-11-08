using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Market
{
    public class LotHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI price;
        [SerializeField] private GameObject lockObject;
        [SerializeField] private LotDataContainer lotDataContainer;
        [SerializeField] private Image backGroundImage;
        
        private MoneyHandler _moneyHandler;
        private Button _lotButton;
        private Lot _currentLot;
        private int _id;

        private void Start()
        {
            _lotButton = GetComponent<Button>();
            
            if (_lotButton != null)
            {
                _lotButton.onClick.AddListener(OnClick); 
            }
            else
            {
                Debug.LogError("Button component not found.");
            }
        }

        public void SetLotValues(Lot currentLot, int id, MoneyHandler moneyHandler)
        {
            _id = id;
            _currentLot = currentLot;
            _moneyHandler = moneyHandler;
            price.text = $"▲{currentLot.price}";
            price.gameObject.SetActive(currentLot.isLocked);
            lockObject.gameObject.SetActive(currentLot.isLocked);
            
            if (backGroundImage != null)
            {
                backGroundImage.sprite = currentLot.image;
            }
            else
            {
                Debug.Log("Image component not found.");
            }
        }

        void OnClick()
        {
            bool bgIsChanged = false;

            if (_currentLot.isLocked) //заблокирован
            {
                if (_currentLot.price <= lotDataContainer.money)//хватает денег
                {
                    lotDataContainer.money -= _currentLot.price;
                    lotDataContainer.lots[_id].isLocked = false;
                    _currentLot.isLocked = false;
                    
                    price.gameObject.SetActive(false);
                    lockObject.gameObject.SetActive(false);
                    _moneyHandler.SetMoneyText(lotDataContainer.money);
                    
                    lotDataContainer.activeBackground = _currentLot.image;
                    lotDataContainer.activeID = _id;
                    bgIsChanged = true;
                }
                else//денег не достаточно
                {
                    _moneyHandler.PlayShakeAnimation();
                }
            }
            else//не заблокирован
            {
                if (_id == 0) //первый бг дефолтный, без спрайта
                {
                    lotDataContainer.activeBackground = null;
                }
                else
                {
                    lotDataContainer.activeBackground = _currentLot.image;
                }

                lotDataContainer.activeID = _id;
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
