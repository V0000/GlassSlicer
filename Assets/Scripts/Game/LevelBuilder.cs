using MeshGenerators;
using UnityEngine;

namespace Game
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private MeshGenerator meshGenerator;
        [SerializeField] private GameObject pinParent;
        [SerializeField] private GameObject pinPrefab;

        public void BuildLevel(LevelData levelData)
        {
            meshGenerator.Build(levelData.shapes);
            foreach (var pinPosition in levelData.pins)
            {
                GameObject instance = Instantiate(pinPrefab, pinPosition, Quaternion.identity);
                instance.transform.SetParent(pinParent.transform);
            }
        }
    }
}
