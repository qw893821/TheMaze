using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class MeshCreator : MonoBehaviour {
    // Use this for initialization
    void Start()
    {
        transform.gameObject.AddComponent<MeshFilter>();
        transform.gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        Vector3 v1 = new Vector3(0, 0, 0);
        Vector3 v2 = new Vector3(1, 0, 1);
        Vector3 v3 = new Vector3(1, 0, -1);
        mesh.vertices = new Vector3[] { v1, v2, v3 };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
        mesh.triangles = new int[] { 0, 1, 2 };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
