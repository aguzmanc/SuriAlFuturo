using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterMovement : MonoBehaviour 
{

    MeshFilter _filter;

    public float Scale = 1f;
    public Vector2 Displacement;
    public float Height = 1f;
    public Vector2 MotionVector;

	void Start () 
    {
        _filter = GetComponent<MeshFilter>();
	}
	


	void Update () 
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int i = 0;
        while (i < vertices.Length) {
            Vector3 v = vertices[i];
            vertices[i] = new Vector3(v.x, Mathf.PerlinNoise((v.x+Displacement.x)*Scale, (v.z+Displacement.y)*Scale) * Height, v.z);
            i++;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        Displacement += MotionVector;


	}
}
