using System.IO;
using LevelsData;
using Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    public class DataLoaderSaver
    {
        private readonly string _levelDataName = "LevelsData.json";
        private readonly string _gameDataName = "GameData.json";

        string _levelDataFilePath = Application.dataPath + "/LevelsData.json";
        string _gameDataFilePath = Application.dataPath + "/GameData.json";

        public void SaveData(GameInfo gameInfo)
        {
            string streamingAssetsFilePath = Path.Combine(Application.streamingAssetsPath, _gameDataName);
            string persistentDataFilePath = Path.Combine(Application.persistentDataPath, _gameDataName);

            string jsonString = JsonConvert.SerializeObject(gameInfo, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            File.WriteAllText(persistentDataFilePath, jsonString);
            Debug.Log($"JSON saved into {persistentDataFilePath}");
            //File.WriteAllText(streamingAssetsFilePath, jsonString);
            //Debug.Log($"JSON saved into {streamingAssetsFilePath}");
        }

        public LevelData[] LoadLevelData()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, _levelDataName);
            string jsonString;

            var reader = new WWW(filePath);
            while (!reader.isDone)
            {
            }

            jsonString = reader.text;

            SerializableLevelsData serializableLevelsData =
                JsonConvert.DeserializeObject<SerializableLevelsData>(jsonString);
            var levelData = serializableLevelsData.levelData;
            Debug.Log("LevelData JSON loaded");

            return levelData;
        }

        public GameInfo LoadGameData()
        {
            string streamingAssetsFilePath = Path.Combine(Application.streamingAssetsPath, _gameDataName);
            string persistentDataFilePath = Path.Combine(Application.persistentDataPath, _gameDataName);
            string jsonString;
            if (File.Exists(persistentDataFilePath)) // если есть файл с прогрессом игрока, подгружаем его, если нет - парсим дефолтный.
            {
                jsonString = File.ReadAllText(persistentDataFilePath);
                Debug.Log("GameData file founded in Application.persistentDataPath");
            }
            else
            {
                var reader = new WWW(streamingAssetsFilePath);
                while (!reader.isDone)
                {
                }

                jsonString = reader.text;
            }


            GameInfo serializableGameInfo = JsonConvert.DeserializeObject<GameInfo>(jsonString);
            //ресейвим в  Application.persistentDataPath
            SaveData(serializableGameInfo);
            Debug.Log("GameData JSON loaded");

            return serializableGameInfo;
        }

        public void DeletePersistentDataFile() //удаляем файл прогресса для сброса. Дефолтный файл подгрузится, когда не будет найден локальный.
        {
            string persistentDataFilePath = Path.Combine(Application.persistentDataPath, _gameDataName);
            if (File.Exists(persistentDataFilePath))
            {
                File.Delete(persistentDataFilePath);
                Debug.Log("Файл успешно удален: " + persistentDataFilePath);
            }
            else
            {
                Debug.LogWarning("Файл не найден: " + persistentDataFilePath);
            }
        }
    }
}