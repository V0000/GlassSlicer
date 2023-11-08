using LevelsData;
using UnityEngine;

namespace Game
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private LevelBuilder levelBuilder;
        
        private LevelData[] _levelsData;
        private GameInfo _gameInfo;

        void Start()
        {
            DataLoaderSaver dataLoaderSaver = new DataLoaderSaver();
            _levelsData = dataLoaderSaver.LoadLevelData();
            _gameInfo = dataLoaderSaver.LoadGameData();
            Debug.Log($"currentLevelNumber - {_gameInfo.currentLevelNumber}");
        
            levelBuilder.BuildLevel(_levelsData[_gameInfo.currentLevelNumber]);
        }

    }
}
