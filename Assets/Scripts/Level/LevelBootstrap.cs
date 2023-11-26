using Data;
using LevelsData;
using UnityEngine;
using Zenject;

namespace Game
{
    public class LevelBootstrap : MonoBehaviour
    {
        private LevelBuilder _levelBuilder;
        private LevelData[] _levelsData;
        private GameInfo _gameInfo;
        private DataLoaderSaver _dataLoaderSaver;

        [Inject]
        private void Initialize(DataLoaderSaver dataLoaderSaver, LevelBuilder levelBuilder)
        {
            _dataLoaderSaver = dataLoaderSaver;
            _levelBuilder = levelBuilder;
        }

        void Start()
        {
            _levelsData = _dataLoaderSaver.LoadLevelData();
            _gameInfo = _dataLoaderSaver.LoadGameData();
            Debug.Log($"Loaded level number - {_gameInfo.currentLevelNumber}");

            _levelBuilder.BuildLevel(_levelsData[_gameInfo.currentLevelNumber]);
        }
    }
}