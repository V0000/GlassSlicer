using TMPro;
using UnityEngine;

namespace Game
{
    public class FinalScreen : MonoBehaviour
    {
        #region Private variables
        
        [SerializeField] private GameObject winImage;
        [SerializeField] private GameObject loseImage;
        [SerializeField] private GameObject nextLevelBtn;
        [SerializeField] private GameObject goToMapBtn;
        [SerializeField] private TextMeshProUGUI moneysForWinText;
        
        #endregion

        public void IsWin(bool isWin, bool isLastLevel, int money) 
        {
            winImage.SetActive(isWin);
            loseImage.SetActive(!isWin);
            nextLevelBtn.SetActive(isWin);
            goToMapBtn.SetActive(false);
            moneysForWinText.gameObject.SetActive(isWin);
            moneysForWinText.text = $"+{money}▲";

            if (isLastLevel) // Запрещаем переходить на след лвл, если лвл последний
            {
                nextLevelBtn.SetActive(false);
                goToMapBtn.SetActive(true);
            }
        }
    
    }
}
