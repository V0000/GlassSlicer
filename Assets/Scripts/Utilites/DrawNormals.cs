using UnityEngine;

[ExecuteInEditMode]
//Класс для отладки генерации мешей
public class DrawNormals : MonoBehaviour
{
    [SerializeField] private float normalLength = 0.1f; // Длина отображаемых нормалей
    
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
            return;

        Mesh mesh = meshFilter.sharedMesh;

        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(vertices[i], vertices[i] + normals[i] * normalLength);
        }
    }
#endif
}