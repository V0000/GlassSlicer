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
        public LevelData levelData;
        [Space(50)] [Range(0, 99)] public int levelNumber = 0;
        private LevelData[] levelsData;
        private string _levelDataName = "LevelsData.json";
    
        private LevelData levelDataTEMP;
    
    
        [EditorButton("Next Level")]
        public void LoadNextLevel()
        {
            SaveJson();
            levelNumber = Mathf.Clamp(levelNumber+1, 0, 99);
            LoadJson();
        }
    
        [EditorButton("Previous Level")]
        public void LoadPreviousLevel()
        {
            SaveJson();
            levelNumber = Mathf.Clamp(levelNumber-1, 0, 99);
            LoadJson();

        }
    
        [EditorButton("Save JSON")]
        public void SaveJson()
        {
            levelsData[levelNumber] = levelData;
        
            SerializableLevelsData serializableLevelsData = new SerializableLevelsData();
            serializableLevelsData.levelData = levelsData;
        
            string filePath = Path.Combine(Application.streamingAssetsPath, _levelDataName);
        
            string jsonString = JsonConvert.SerializeObject(serializableLevelsData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        
            File.WriteAllText(filePath, jsonString);
            Debug.Log($"JSON saved into {filePath}");
        }
    
        [EditorButton("Load JSON")]
        public void LoadJson()
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, _levelDataName);
            string jsonString = File.ReadAllText(filePath);
            SerializableLevelsData serializableLevelsData = JsonConvert.DeserializeObject<SerializableLevelsData>(jsonString);
            levelsData = serializableLevelsData.levelData;
            levelData = levelsData[levelNumber];
            Debug.Log($"JSON loaded from {filePath}");
        }
    
        [EditorButton("Copy level Data")]
        public void CopylevelData()
        {
            levelDataTEMP = levelData;
        }
        [EditorButton("Paste level Data")]
        public void PastelevelData()
        {
            levelData = levelDataTEMP;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() //тут схематично отрисовываем внешний вид уровня
        {
            if (levelsData.Length < levelNumber)
            {
                Debug.LogWarning("Mismatch in the number of levels!!!");
            }

            var shapes = levelData.shapes;
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
                    Handles.Label(shape.vertices[i]+ Vector3.up*0.2f, i.ToString());
                    Gizmos.DrawLine(lastPoint, shape.vertices[i]);
                    lastPoint = shape.vertices[i];
                }

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(shape.center, 0.1f);
                Handles.Label(shape.center + Vector3.up*0.2f, j.ToString());
            }
        
            var pins = levelData.pins;
            for (int j = 0; j < pins.Length; j++)
            {
                var pin = pins[j];
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(pin, 0.1f);
                Handles.Label(pin+ Vector3.up*0.2f, j.ToString());
            }
        
            var lines = levelData.answer;
            for (int j = 0; j < lines.Length; j+=2)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(lines[j], lines[j+1]);
                Handles.Label(lines[j] + Vector3.up*0.2f, j.ToString());
                Handles.Label(lines[j+1] + Vector3.up*0.2f, (j+1).ToString());
            }
        }
#endif
    }
}