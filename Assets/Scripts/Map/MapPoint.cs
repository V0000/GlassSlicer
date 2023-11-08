using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class MapPoint : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelNumberText;
        [SerializeField] private GameObject maxMarker;
        [SerializeField] private GameObject lockSign;
        public MapManager mapManager;
    
        private Button _button;
        private int _lvlNumber;

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
            mapManager.LoadLevel(_lvlNumber);
            Debug.LogWarning($"{_lvlNumber} is loading");
        }
    }
}