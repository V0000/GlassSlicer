using MeshGenerators;
using UnityEngine;
using Zenject;

namespace Game
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private GameObject pinParent;
        [SerializeField] private GameObject pinPrefab;
        
        private MeshGenerator _meshGenerator;

        [Inject]
        private void Initialize(MeshGenerator meshGenerator)
        {
            _meshGenerator = meshGenerator;
        }
        public void BuildLevel(LevelData levelData)
        {
            _meshGenerator.Build(levelData.shapes);
            GeneratePins(levelData.pins);
        }
        
        private void GeneratePins(Vector3[] pins)
        {
            foreach (var pinPosition in pins)
            {
                GameObject instance = Instantiate(pinPrefab, pinPosition, Quaternion.identity);
                instance.transform.SetParent(pinParent.transform);
            }
        }
    }
}
