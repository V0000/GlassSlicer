using System.Collections;
using System.Collections.Generic;
using MeshGenerators;
using UnityEngine;
[System.Serializable]
public class LevelData
{
    public Shape[] shapes;
    public Vector3[] answer;
    public Vector3[] pins;
    public int moves;
}
