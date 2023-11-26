using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Map
{
    public class MapPoint : MonoBehaviour
    {
        #region Private variables
        
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private GameObject maxMarker;
        [SerializeField] private GameObject lockSign;
        
    
        private Button _button;
        private int _lvlNumber;
        private MapManager _mapManager;
        
        #endregion

        #region Public methods

        public void SetText(int number)
        {
            levelNumberText.gameObject.SetActive(true);
            maxMarker.SetActive(false);
            lockSign.SetActive(false);
            
            _lvlNumber = number;
            levelNumberText.text = _lvlNumber.ToString();
        }
    
        public void SetActual()
        {
            levelNumberText.gameObject.SetActive(true);
            maxMarker.SetActive(true);
            lockSign.SetActive(false);
        }
        
        public void SetMapManager(MapManager mapManager)
        {
            _mapManager = mapManager;
        }
    
        public void SetLocked()
        {
            Button button = GetComponent<Button>();
        
            levelNumberText.gameObject.SetActive(false);
            maxMarker.SetActive(false);
            lockSign.SetActive(true);
            button.interactable = false;
        }

        public void GoToLevel()
        {
            _mapManager.LoadLevel(_lvlNumber);
            Debug.LogWarning($"{_lvlNumber} is loading");
        }
        
        #endregion
    }
}