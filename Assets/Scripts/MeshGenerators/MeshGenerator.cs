using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MeshGenerators
{
    //класс занимается генерацией набора многоугольников для уровня
    public class MeshGenerator : MonoBehaviour
    {
        [SerializeField] private Material[] materials;
        
        private const float Thickness = 0.6f;
        private GameObject _prefabToInstantiate;
        private const string PrefabPath = "Prefabs/SlicableShape";
        private Vector3[] _backVertices;
        private Vector3 _backCenter;
        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;
        private MeshRenderer _meshRenderer;


        
        public void Build(Shape[] shapes)
        {
            if (shapes == null)
            {
                Debug.LogWarning("There is no shapes for instantiate");
                return;
            }

            if (materials.Length == 0)
            {
                Debug.LogWarning("There is no materials");
                return;
            }

            foreach (var shape in shapes)
            {
                BuildShape(shape);
            }
            
            
        }

        private void BuildShape(Shape shape) // мастер-метод, занимается сборкой конкретного меша
        {
            _prefabToInstantiate = Resources.Load<GameObject>(PrefabPath);
            GameObject instance = Instantiate(_prefabToInstantiate, Vector3.zero, Quaternion.identity);

            _meshFilter = instance.GetComponent<MeshFilter>();
            _meshCollider = instance.GetComponent<MeshCollider>();
            _meshRenderer = instance.GetComponent<MeshRenderer>();

            if (shape.vertices.Length < 3)
            {
                Debug.LogWarning("There must be at least 3 vertices to create a polygon.");
                return;
            }

            CombineInstance[] combineInstances = new CombineInstance[2 + shape.vertices.Length];
            _backCenter = new Vector3(0, 0, Thickness) + shape.center;
            _backVertices = CalcBackVertices(shape);

            //Меш передней грани
            combineInstances[0].mesh = GenerateMesh(shape.vertices, shape.center);

            //Меш задней грани
            combineInstances[1].mesh = GenerateMesh(_backVertices, _backCenter);


            //Меш боковых граней
            for (int i = 0; i < shape.vertices.Length; i++)
            {
                int vertice1 = i;
                int back_i = shape.vertices.Length - i < 0 ? 0 : shape.vertices.Length - i;
                int vertice2 = back_i == shape.vertices.Length ? 0 : back_i;
                int vertice3 = vertice2 - 1 < 0 ? shape.vertices.Length - 1 : vertice2 - 1;
                int vertice4 = i + 1 == shape.vertices.Length ? 0 : i + 1;

                Vector3[] edgeVertices = new Vector3[4]
                {
                    shape.vertices[vertice1], _backVertices[vertice2], _backVertices[vertice3], shape.vertices[vertice4]
                };
                combineInstances[i + 2].mesh = GenerateMesh(edgeVertices, Vector3.zero);
            }

            var mesh = new Mesh();
            mesh.CombineMeshes(combineInstances, true, false);
            mesh.name = "GameShape";
            _meshFilter.mesh = mesh;
            _meshCollider.sharedMesh = mesh;
            
            int randomIndex = Random.Range(0, materials.Length);
            Material randomMaterial = materials[randomIndex];
            _meshRenderer.material = randomMaterial;

            //RoundEdges();
            //AddRigidbody();
        }

        private void AddRigidbody() 
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();

                rb.mass = 1.0f;
                rb.drag = 0.0f;
                rb.angularDrag = 0.05f;
                rb.useGravity = true;
                rb.isKinematic = false;
            }
        }

        private Vector3[] CalcBackVertices(Shape shape) // вычисляем заднюю стенку фигуры
        {
            Vector3[] bVertices = new Vector3[shape.vertices.Length];
            Array.Copy(shape.vertices, bVertices, shape.vertices.Length);
            Array.Reverse(bVertices);
            Vector3 offset = new Vector3(0, 0, Thickness);
            for (int i = 0; i < bVertices.Length; i++)
            {
                bVertices[i] += offset;
            }

            Vector3 zeroVector = bVertices[bVertices.Length - 1];
            for (int i = bVertices.Length - 1; i > 0; i--)
            {
                bVertices[i] = bVertices[i - 1];
            }

            bVertices[0] = zeroVector;
            return bVertices;
        }

        private Mesh GenerateMesh(Vector3[] polygonVertices, Vector3 centralVert)
        {
            Mesh mesh = new Mesh();
            int[] triangles;

            // Создаем массив вершин для меша


            // Создаем массив треугольников (индексы вершин)
            if (centralVert != Vector3.zero)
            {
                Vector3[] polygonVerticesAndCenter = new Vector3[polygonVertices.Length + 1];
                for (int i = 0; i < polygonVertices.Length; i++)
                {
                    polygonVerticesAndCenter[i] = polygonVertices[i];
                }

                polygonVerticesAndCenter[polygonVerticesAndCenter.Length - 1] = centralVert;
                mesh.vertices = polygonVerticesAndCenter;
                triangles = GenerateTriangles(polygonVerticesAndCenter.Length);
            }
            else
            {
                mesh.vertices = polygonVertices;
                triangles = new[] { 0, 1, 3, 1, 2, 3 };
            }

            mesh.triangles = triangles;

            // Рассчитываем нормали для вершин
            mesh.RecalculateNormals();
            mesh.uv = CalculateUV(mesh);
            return mesh;
        }


        private int[] GenerateTriangles(int vertexCount) // вычисляем порядок треугольников
        {
            int[] triangles = new int[(vertexCount - 1) * 3];
            int index = 0;

            for (int i = 0; i < vertexCount - 1; i++)
            {
                triangles[index++] = i;
                triangles[index++] = i + 1 != vertexCount - 1 ? i + 1 : 0;
                triangles[index++] = vertexCount - 1;
            }

            return triangles;
        }

        private Vector2[] CalculateUV(Mesh mesh) // считаем UV координаты
        {
            Vector3[] vertices = mesh.vertices;

            Vector2[] uv = new Vector2[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[0].z == vertices[1].z)
                {
                    // используем позицию вершины для UV-координат
                    uv[i] = new Vector2(vertices[i].x, vertices[i].y);
                }
                else
                {
                    uv[i] = new Vector2(vertices[i].y, vertices[i].z);
                }
            }

            return uv;
        }

/*#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            foreach (var shape in shapesForDrawGizmos)
            {
                if (shape.vertices == null)
                {
                    return;
                }

                Gizmos.color = Color.yellow;

                foreach (var point in shape.vertices)
                {
                    Gizmos.DrawSphere(point + shape.spawnPosition, 0.1f);
                }

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(shape.center + shape.spawnPosition, 0.1f);
            }
        }
#endif*/
    }
}