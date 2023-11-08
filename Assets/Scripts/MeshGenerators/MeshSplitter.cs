using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Скрипт может определить, что меш разделен визуально и создать два разных меша.
public class MeshSplitter : MonoBehaviour
{
    public GameObject shape;

    private MeshFilter meshFilter;

    private int[] triangles;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            meshFilter = shape.GetComponent<MeshFilter>();
            triangles = meshFilter.mesh.triangles;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                int vertexIndexA = triangles[i];
                int vertexIndexB = triangles[i + 1];
                int vertexIndexC = triangles[i + 2];
                //Debug.Log($"{vertexIndexA}:{vertexIndexB}:{vertexIndexC}");
            }

            foreach (var VARIABLE in meshFilter.mesh.vertices)
            {
                Debug.Log(VARIABLE);
            }
            
            Debug.Log($"количество точек треугольников: {triangles.Length}, количество треугольников: {triangles.Length/3}, точки: {meshFilter.mesh.vertices.Length}");
            Debug.Log(HasDisconnectedParts(meshFilter.mesh));
            Debug.Log($"количество точек треугольников: {triangles.Length}, количество треугольников: {triangles.Length/3}, точки: {meshFilter.mesh.vertices.Length}");
        }
    }



    public bool HasDisconnectedParts(Mesh mesh)
    {
        // Получаем массив вершин и массив треугольников меша
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        MergeSamePoints(vertices, triangles);
        // Создаем массив, чтобы отслеживать, были ли вершины посещены
        bool[] visited = new bool[vertices.Length];

        // Инициализируем все вершины как не посещенные
        for (int i = 0; i < visited.Length; i++)
        {
            visited[i] = false;
        }

        // Начинаем новый обход из нулевой вершины
        DFS(triangles, visited, 0);

        int countTrue = visited.Count(x => x == true);
        int countFalse = visited.Count(x => x == false);
   
        
        // Проверяем, остались ли не посещенные вершины (несоединенные части)
        for (int i = 0; i < visited.Length; i++)
        {
            if (!visited[i])
            {
                return true; // Есть несоединенные части
            }
        }

        return false; // Нет несоединенных частей
    }
    public void MergeSamePoints(Vector3[] vertices,  int[] triangles)
    {
        int deleted = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = i; j < vertices.Length; j++)
            {
                if (vertices[i] == vertices[j] && i != j)
                {
                    deleted++;
                    for (int id = 0; id < triangles.Length; id++)
                    {
                        if (triangles[id] == j)
                        {
                            triangles[id] = i;
                        }
                    }
                }
            }
        }
        Debug.Log($"удалено: {deleted}");
    }
    private void DFS(int[] triangles, bool[] visited, int vertexIndex)
    {
        visited[vertexIndex] = true;

        // Проходим по смежным вершинам и рекурсивно вызываем DFS
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int vertexA = triangles[i];
            int vertexB = triangles[i + 1];
            int vertexC = triangles[i + 2];

            if (vertexA == vertexIndex)
            {
                if (!visited[vertexB])
                {
                    DFS(triangles, visited, vertexB);
                }
                if (!visited[vertexC])
                {
                    DFS(triangles, visited, vertexC);
                }
                
            }

            if (vertexB == vertexIndex)
            {
                if (!visited[vertexA])
                {
                    DFS(triangles, visited, vertexA);
                }
                if (!visited[vertexC])
                {
                    DFS(triangles, visited, vertexC);
                }
                
            }

            if (vertexC == vertexIndex)
            {
                if (!visited[vertexA])
                {
                    DFS(triangles, visited, vertexA);
                }
                if (!visited[vertexB])
                {
                    DFS(triangles, visited, vertexB);
                }
                
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (meshFilter == null)
        {
            return;
        }

        Vector3[] vertices = meshFilter.mesh.vertices;
        int[] triangles = meshFilter.mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 centroid = (vertices[triangles[i]] + vertices[triangles[i + 1]] + vertices[triangles[i + 2]]) / 3f;
            string triangleNumber = (i / 3).ToString(); // Номер треугольника
            
            Handles.Label(centroid, triangleNumber); // Отображаем номер треугольника
        }
    }
#endif
}