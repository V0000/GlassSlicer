using System;
using Map;
using UnityEngine;

namespace Market
{
    public class MarketBuilder : MonoBehaviour
    {
        [SerializeField] private LotHandler lotPrefab;
        [SerializeField] private GameObject lotParent;
        [SerializeField] private LotDataContainer lotData;
        [SerializeField] private ScrollMover scrollMover;
        [SerializeField] private MoneyHandler moneyHandler;
        
        void Start()
        {
            int id = 0;
            foreach (Lot lot in lotData.lots)
            {
                //Debug.Log("Price: " + lot.price);
                //Debug.Log("Is Locked: " + lot.isLocked);
            
                GameObject instantiatedObject = Instantiate(lotPrefab.gameObject);
                instantiatedObject.transform.SetParent(lotParent.transform, false);

                LotHandler instantiatedLot = instantiatedObject.GetComponent<LotHandler>();
                instantiatedLot.SetLotValues(lot, id, moneyHandler);
                id++;

            }

            moneyHandler.SetMoneyText(lotData.money);
            float scrollPosition = (float)lotData.activeID / (float)(lotData.lots.Length-1);
            scrollMover.SetScrollValueY(scrollPosition);
        }


    }
}
