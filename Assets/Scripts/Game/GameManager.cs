using System.Collections;
using Ads;
using DynamicMeshCutter;
using LevelsData;
using TMPro;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private FinalScreen finalScreen;
        [SerializeField] private TextMeshProUGUI movesText;
        [SerializeField] private TextMeshProUGUI hintsText;
        [SerializeField] private MouseBehaviour mouseBehaviour;
        [SerializeField] private PinsCounter pinsCounter;
        [SerializeField] private LineDrawer lineDrawer;
    
        [SerializeField] private LotDataContainer lotDataContainer;
        [SerializeField] private SpriteRenderer backGroundImage;
        [SerializeField] private HintRewardedAdsButton hintRewardedAdsButton;
    
        private float _timeSinceLastCall = 0f;
        private readonly float _checkInterval = 0.1f;
        private DataLoaderSaver _dataLoaderSaver;
        private GameInfo _gameInfo;
        private LevelData[] _levelsData;
        private int _moves;
        private int _hints;
        private void Start()
        {
            _dataLoaderSaver = new DataLoaderSaver();
            _levelsData = _dataLoaderSaver.LoadLevelData();
            _gameInfo = _dataLoaderSaver.LoadGameData();
            _moves = _levelsData[_gameInfo.currentLevelNumber].moves;
            _hints = _gameInfo.hints;

            mouseBehaviour.cutterIsLocked = false;
            mouseBehaviour.OnObjectCutOff += OnStep;
            ButtonManager.NextLevelPressed += OnNextLvl;
            
            finalScreen.gameObject.SetActive(false);
            movesText.SetText(_moves.ToString());
            hintsText.SetText(_hints.ToString());
        
            if (_hints==0)
            {
                hintsText.SetText("+");
            }
        
            backGroundImage.sprite = lotDataContainer.activeBackground;
        }
        void Update()
        {
            _timeSinceLastCall += Time.deltaTime;
        }
    
        public void UseHint()
        {
            if (_hints>0)
            {
                _hints--;
            
            
                lineDrawer.ShowLines(_levelsData[_gameInfo.currentLevelNumber].answer);
                Debug.Log("Hint is used");
            }
            else
            {
                hintRewardedAdsButton.ShowAd();
            }
        
            hintsText.SetText(_hints.ToString());
            _gameInfo.hints = _hints;
            _dataLoaderSaver.SaveData(_gameInfo);
        
            if (_hints==0)
            {
                hintsText.SetText("+");
            }
        }
        public void AddHint(int numHints)
        {
            _hints += numHints;
        
            hintsText.SetText(_hints.ToString());
            _gameInfo.hints = _hints;
            _dataLoaderSaver.SaveData(_gameInfo);
        }
    
        public void OnStep() //происходит когда произошел разрез
        {
        
            if (_timeSinceLastCall >= _checkInterval)
            {
                StartCoroutine(DelayedMethod());
                _timeSinceLastCall = 0f;
            }
        }
    
        public void OnLose()
        {
            //DropAllShapes();s
            finalScreen.gameObject.SetActive(true);
            finalScreen.IsWin(false, false, 0);
            mouseBehaviour.cutterIsLocked = true;
        
            Debug.Log("Game is loss!");        
        }
    
        public void OnWin()
        {
            finalScreen.gameObject.SetActive(true);
            Debug.Log($"_gameInfo.currentLevelNumber = {_gameInfo.currentLevelNumber}");   
            Debug.Log($"_levelsData.Length = {_levelsData.Length}");  
            finalScreen.IsWin(true, _levelsData.Length - 1 == _gameInfo.currentLevelNumber, _levelsData[_gameInfo.currentLevelNumber].moves);
            mouseBehaviour.cutterIsLocked = true;
            lotDataContainer.money += _levelsData[_gameInfo.currentLevelNumber].moves;
        
            Debug.Log("Game is won!");
        
        }
        private void OnNextLvl()
        {
            if (_gameInfo.currentLevelNumber < _levelsData.Length)
            {
                _gameInfo.currentLevelNumber++;
            }
        
            if (_gameInfo.maxLevelNumber < _gameInfo.currentLevelNumber)
            {
                _gameInfo.maxLevelNumber =  _gameInfo.currentLevelNumber;
            }
            _dataLoaderSaver.SaveData(_gameInfo);
            Debug.Log($"currentLevelNumber - {_gameInfo.currentLevelNumber}, maxLevelNumber - {_gameInfo.maxLevelNumber}");
        
        }
    
        //метод используется в тестировании
        /*
        public void DropAllShapes()
        {
            GameObject[] shapes = GameObject.FindGameObjectsWithTag("Shape");

            foreach (GameObject shape in shapes)
            {
                Rigidbody rb = shape.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }
            }
        
            GameObject[] pins = GameObject.FindGameObjectsWithTag("Pin");
        
            foreach (GameObject pin in pins)
            {
                Rigidbody rb = pin.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                }
            }
        
        }
        */
    
        IEnumerator DelayedMethod()
        {
            yield return new WaitForSeconds(1f); //разрез происходит с задержкой
        
            _moves--;
            movesText.SetText(_moves.ToString());
            int pinCount = pinsCounter.GetCountKinematicChilds();
            //Debug.Log($"pinsCount - {pinCount}");
            if(pinCount == 0 && _moves >= 0)
            {
                OnWin();
            }
            if(pinCount != 0 && _moves == 0)
            {
                OnLose();
            }

        }

    }
}
