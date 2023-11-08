using UnityEngine;

namespace LevelsData
{
    [CreateAssetMenu(fileName = "LevelsData", menuName = "Levels Data")]
    public class AllLevelsData : ScriptableObject
    {
        public LevelData[] levelData;
    
    }
}