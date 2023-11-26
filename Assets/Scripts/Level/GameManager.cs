using System.Collections;
using Ads;
using Data;
using DynamicMeshCutter;
using LevelsData;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        #region Private variables

        [SerializeField] private TextMeshProUGUI movesText;
        [SerializeField] private TextMeshProUGUI hintsText;
        [SerializeField] private PinsCounter pinsCounter;
        [SerializeField] private SpriteRenderer backGroundImage;

        private HintBaseRewardedAdsButton _hintBaseRewardedAdsButton;
        private MouseBehaviour _mouseBehaviour;
        private FinalScreen _finalScreen;
        private HintLineDrawer _hintLineDrawer;
        private ItemDataContainer _itemDataContainer;
        private float _timeSinceLastCall = 0f;
        private DataLoaderSaver _dataLoaderSaver;
        private GameInfo _gameInfo;
        private LevelData[] _levelsData;
        private int _moves;
        private int _hints;

        private const float CheckInterval = 0.1f;
        private const float SliceDelay = 1f;

        #endregion

        [Inject]
        private void Initialize(DataLoaderSaver dataLoaderSaver, FinalScreen finalScreen,
            HintBaseRewardedAdsButton hintBaseRewardedAdsButton, MouseBehaviour mouseBehaviour,
            ItemDataContainer itemDataContainer, HintLineDrawer hintLineDrawer)
        {
            _dataLoaderSaver = dataLoaderSaver;
            _finalScreen = finalScreen;
            _hintBaseRewardedAdsButton = hintBaseRewardedAdsButton;
            _mouseBehaviour = mouseBehaviour;
            _itemDataContainer = itemDataContainer;
            _hintLineDrawer = hintLineDrawer;
            
        }

        #region Private methods

        private void Start()
        {
            LoadAndSetGameData();
            SetupMouseAndNavigation();
            UpdateUIValues();
            _finalScreen.gameObject.SetActive(false);
            backGroundImage.sprite = _itemDataContainer.activeBackground;
        }

        private void Update()
        {
            IncrementTimeSinceLastCall();
        }

        private void LoadAndSetGameData()
        {
            _levelsData = _dataLoaderSaver.LoadLevelData();
            _gameInfo = _dataLoaderSaver.LoadGameData();
            _moves = _levelsData[_gameInfo.currentLevelNumber].moves;
            _hints = _gameInfo.hints;
        }
        
        private void SetupMouseAndNavigation()
        {
            _mouseBehaviour.cutterIsLocked = false;
            _mouseBehaviour.OnObjectCutOff += OnStep;
            NavigationButtonsHandler.OnNextLevelPressed += OnNextLvl;
        }
        
        private void UpdateUIValues()
        {
            movesText.SetText(_moves.ToString());
            hintsText.SetText(_hints.ToString());
            if (_hints == 0)
            {
                hintsText.SetText("+");
            }
            
        }
        
        private void IncrementTimeSinceLastCall()
        {
            _timeSinceLastCall += Time.deltaTime;
        }

        #endregion

        #region Public methods

        public void UseHint()
        {
            if (_hints > 0)
            {
                _hints--;
                _hintLineDrawer.ShowLines(_levelsData[_gameInfo.currentLevelNumber].answer);
                Debug.Log("Hint is used");
            }
            else
            {
                _hintBaseRewardedAdsButton.ShowAd();
            }

            hintsText.SetText(_hints.ToString());
            _gameInfo.hints = _hints;
            _dataLoaderSaver.SaveGameData(_gameInfo);

            if (_hints == 0)
            {
                hintsText.SetText("+");
            }
        }

        public void AddHint(int numHints)
        {
            _hints += numHints;

            hintsText.SetText(_hints.ToString());
            _gameInfo.hints = _hints;
            _dataLoaderSaver.SaveGameData(_gameInfo);
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

        #endregion

        #region Event methods

        public void OnStep() //происходит когда произошел разрез
        {
            if (_timeSinceLastCall >= CheckInterval)
            {
                StartCoroutine(DelayedMethod());
                _timeSinceLastCall = 0f;
            }
        }

        public void OnLose()
        {
            //DropAllShapes();s
            _finalScreen.gameObject.SetActive(true);
            _finalScreen.IsWin(false, false, 0);
            _mouseBehaviour.cutterIsLocked = true;

            Debug.Log("Game is loss!");
        }

        public void OnWin()
        {
            _finalScreen.gameObject.SetActive(true);
            Debug.Log($"_gameInfo.currentLevelNumber = {_gameInfo.currentLevelNumber}");
            Debug.Log($"_levelsData.Length = {_levelsData.Length}");
            _finalScreen.IsWin(true, _levelsData.Length - 1 == _gameInfo.currentLevelNumber,
                _levelsData[_gameInfo.currentLevelNumber].moves);
            _mouseBehaviour.cutterIsLocked = true;
            _itemDataContainer.money += _levelsData[_gameInfo.currentLevelNumber].moves;

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
                _gameInfo.maxLevelNumber = _gameInfo.currentLevelNumber;
            }

            _dataLoaderSaver.SaveGameData(_gameInfo);
            Debug.Log(
                $"currentLevelNumber - {_gameInfo.currentLevelNumber}, maxLevelNumber - {_gameInfo.maxLevelNumber}");
        }

        #endregion


        IEnumerator DelayedMethod()
        {
            yield return new WaitForSeconds(SliceDelay); //разрез происходит с задержкой

            _moves--;
            movesText.SetText(_moves.ToString());
            int pinCount = pinsCounter.GetCount();

            if (pinCount == 0 && _moves >= 0)
            {
                OnWin();
            }

            if (pinCount != 0 && _moves == 0)
            {
                OnLose();
            }
        }
    }
}