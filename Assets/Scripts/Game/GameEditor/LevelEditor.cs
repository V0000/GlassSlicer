using System.IO;
using Kilosoft.Tools;
using LevelsData;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;


namespace GameEditor
{
    //Редактор JSON файла с описанием каждого уровня
    public class LevelEditor : MonoBehaviour
    {
        #region Private variables
        
        [SerializeField] private LevelData levelData;

        [Space(50)] [Range(0, 99)] [SerializeField]
        private int levelNumber = 0;

        private LevelData[] _levelsData;
        private const string LevelDataName = "LevelsData.json";
        private LevelData _levelDataTemp;
        
        private readonly string _filePath = Path.Combine(Application.streamingAssetsPath, LevelDataName);

        #endregion
        
        #region Public methods

        [EditorButton("Next Level")]
        public void LoadNextLevel()
        {
            SaveJson();
            levelNumber = Mathf.Clamp(levelNumber + 1, 0, 99);
            LoadJson();
        }

        [EditorButton("Previous Level")]
        public void LoadPreviousLevel()
        {
            SaveJson();
            levelNumber = Mathf.Clamp(levelNumber - 1, 0, 99);
            LoadJson();
        }

        [EditorButton("Save JSON")]
        public void SaveJson()
        {
            _levelsData[levelNumber] = levelData;

            SerializableLevelsData serializableLevelsData = new SerializableLevelsData();
            serializableLevelsData.levelData = _levelsData;

            

            string jsonString = JsonConvert.SerializeObject(serializableLevelsData, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            File.WriteAllText(_filePath, jsonString);
            Debug.Log($"JSON saved into {_filePath}");
        }

        [EditorButton("Load JSON")]
        public void LoadJson()
        {
            string jsonString = File.ReadAllText(_filePath);
            SerializableLevelsData serializableLevelsData =
                JsonConvert.DeserializeObject<SerializableLevelsData>(jsonString);
            _levelsData = serializableLevelsData.levelData;
            levelData = _levelsData[levelNumber];
            Debug.Log($"JSON loaded from {_filePath}");
        }

        [EditorButton("Copy level Data")]
        public void CopylevelData()
        {
            _levelDataTemp = levelData;
        }

        [EditorButton("Paste level Data")]
        public void PastelevelData()
        {
            levelData = _levelDataTemp;
        }

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos() // схематично отрисовываем внешний вид уровня
        {
            if (_levelsData.Length < levelNumber)
            {
                Debug.LogWarning("Mismatch in the number of levels!!!");
            }

            var shapes = levelData.shapes;
            //отображение многоугольника
            for (int j = 0; j < shapes.Length; j++)
            {
                var shape = shapes[j];
                if (shape.vertices == null)
                {
                    return;
                }

                Gizmos.color = Color.yellow;
                Vector3 lastPoint = shape.vertices[^1];

                for (int i = 0; i < shape.vertices.Length; i++)
                {
                    Gizmos.DrawSphere(shape.vertices[i], 0.1f);
                    Handles.Label(shape.vertices[i] + Vector3.up * 0.2f, i.ToString());
                    Gizmos.DrawLine(lastPoint, shape.vertices[i]);
                    lastPoint = shape.vertices[i];
                }

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(shape.center, 0.1f);
                Handles.Label(shape.center + Vector3.up * 0.2f, j.ToString());
            }

            var pins = levelData.pins;
            //отображение пинов
            for (int j = 0; j < pins.Length; j++) 
            {
                var pin = pins[j];
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(pin, 0.1f);
                Handles.Label(pin + Vector3.up * 0.2f, j.ToString());
            }

            var lines = levelData.answer;
            //отображение линий-ответов
            for (int j = 0; j < lines.Length; j += 2)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(lines[j], lines[j + 1]);
                Handles.Label(lines[j] + Vector3.up * 0.2f, j.ToString());
                Handles.Label(lines[j + 1] + Vector3.up * 0.2f, (j + 1).ToString());
            }
        }
#endif
    }
}