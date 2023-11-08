using System.Collections;
using System.Collections.Generic;
using Game;
using LevelsData;
using Map;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapPoint mapPoint;
    [SerializeField] private GameObject map;
    [SerializeField] private ScrollMover scrollMover;
    
    private LevelData[] _levelsData;
    private GameInfo _gameInfo;
    private DataLoaderSaver _dataLoaderSaver;
    void Start()
    {
        _dataLoaderSaver = new DataLoaderSaver();
        _levelsData = _dataLoaderSaver.LoadLevelData();
        _gameInfo = _dataLoaderSaver.LoadGameData();
        BuildMap();
        float scrollIndex = 1f -  (float)_gameInfo.maxLevelNumber /(float)_levelsData.Length;
        scrollMover.SetScrollValueX(scrollIndex);
        
        Debug.Log($"currentLevelNumber-{_gameInfo.currentLevelNumber}, maxLevelNumber-{_gameInfo.maxLevelNumber}");
    }

    void BuildMap()
    {
        int lvlId = 0;
        foreach (var level in _levelsData)
        {
            GameObject instantiatedObject = Instantiate(mapPoint.gameObject);
            instantiatedObject.transform.SetParent(map.transform, false);
            MapPoint mapPointInst = instantiatedObject.GetComponent<MapPoint>();
            mapPointInst.SetText(lvlId);
            mapPointInst.mapManager = this;
            if (_gameInfo.maxLevelNumber == lvlId)
            {
                mapPointInst.SetActual();
            }
            if (_gameInfo.maxLevelNumber < lvlId)
            {
                mapPointInst.SetLocked();
            }

            lvlId++;
        }
    }
    
    public void LoadLevel(int levelName)
    {
        _gameInfo.currentLevelNumber = levelName;
        _dataLoaderSaver.SaveData(_gameInfo);
        SceneManager.LoadScene("Level");
    }
    
    public void GoHome()
    {
        Debug.Log("GoHome is pressed");
        SceneManager.LoadScene("Menu");
    }

}
