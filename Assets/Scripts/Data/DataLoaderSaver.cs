using System.IO;
using LevelsData;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    public class DataLoaderSaver
    {
        #region Private variables

        private const string LevelDataName = "LevelsData.json";
        private const string GameDataName = "GameData.json";

        private readonly string _levelDataFilePath = Path.Combine(Application.streamingAssetsPath, LevelDataName);
        private readonly string streamingAssetsFilePath = Path.Combine(Application.streamingAssetsPath, GameDataName);
        private readonly string persistentDataFilePath = Path.Combine(Application.persistentDataPath, GameDataName);

        #endregion


        #region Public methods

        public void SaveGameData(GameInfo gameInfo)
        {
            string jsonString = JsonConvert.SerializeObject(gameInfo, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            File.WriteAllText(persistentDataFilePath, jsonString);
            Debug.Log($"JSON saved into {persistentDataFilePath}");
        }

        public GameInfo LoadGameData()
        {
            string jsonString;
            if (File.Exists(
                    persistentDataFilePath)) // если есть файл с прогрессом игрока, подгружаем его, если нет - парсим дефолтный.
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
            SaveGameData(serializableGameInfo);
            Debug.Log("GameData JSON loaded");

            return serializableGameInfo;
        }

        public LevelData[] LoadLevelData()
        {
            string jsonString;

            var reader = new WWW(_levelDataFilePath);
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


        public void DeletePersistentDataFile() //удаляем файл прогресса для сброса. Дефолтный файл подгрузится, когда не будет найден локальный.
        {
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

        #endregion
    }
}