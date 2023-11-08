using System.IO;
using Game;
using Kilosoft.Tools;
using LevelsData;
using Newtonsoft.Json;
using UnityEngine;

namespace GameEditor
{
    //Редактор JSON файла с прогрессом игры
    public class GameInfoEditor : MonoBehaviour
    {
        public GameInfo gameInfo;
    
        private readonly string _gameDataName = "GameData.json";
    
        [EditorButton("Save JSON")]
        public void SaveJson()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, _gameDataName);
            string jsonString = JsonConvert.SerializeObject(gameInfo, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        
            File.WriteAllText(filePath, jsonString);
            Debug.Log($"JSON saved into {filePath}");
        }
    
        [EditorButton("Load JSON")]
        public void LoadJson()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, _gameDataName);
            string jsonString = File.ReadAllText(filePath);
            gameInfo = JsonConvert.DeserializeObject<GameInfo>(jsonString);
            Debug.Log("JSON loaded");
        }
    
        [EditorButton("DropProgress")]
        public void DropProgress()
        {
            DataLoaderSaver dataLoaderSaver = new DataLoaderSaver();
            dataLoaderSaver.DeletePersistentDataFile();
        }
    }
}
