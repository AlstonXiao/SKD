using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// <para>
/// This class is used to split the meshes into faces
/// </para>
/// Updated: 2/4/2019<para/>
/// Author: Resources on Internet<para/>
/// Attached object: None <para/>
/// Currently Unused <para/>
/// </summary>
public class CSGw : MonoBehaviour {
    public Material myMaterial;


    public List<int> allIndices = new List<int>();
    public List<int> newIndices = new List<int>();
    public List<int> restIndices = new List<int>();

    // Use this for initialization
    void Start() {
        SplitMesh();
    }

    void SplitMesh() {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        int[] indices = mesh.triangles;
        Vector3[] verts = mesh.vertices;

        //list all indices
        for (int i = 0; i < indices.Length; i++) {
            allIndices.Add(indices[i]);
            restIndices.Add(indices[i]);
        }

        while (restIndices.Count > 0) {
            newIndices.Clear();
            //Get first triangle
            for (int i = 0; i < 3; i++) {
                newIndices.Add(restIndices[i]);
            }
            for (int i = 1; i < restIndices.Count / 3; i++) {
                if (newIndices.Contains(restIndices[(i * 3) + 0]) || newIndices.Contains(restIndices[(i * 3) + 1]) || newIndices.Contains(restIndices[(i * 3) + 2])) {
                    for (int q = 0; q < 3; q++) {
                        newIndices.Add(restIndices[(i * 3) + q]);
                    }
                }
            }
            restIndices.Clear();
            for (int n = 0; n < allIndices.Count; n++) {
                if (!newIndices.Contains(allIndices[n])) {
                    restIndices.Add(allIndices[n]);
                }
            }
            allIndices.Clear();
            for (int i = 0; i < restIndices.Count; i++) {
                allIndices.Add(restIndices[i]);
            }

            mesh.triangles = restIndices.ToArray();

            Mesh newMesh = new Mesh();
            newMesh.vertices = verts;
            newMesh.triangles = newIndices.ToArray();

            newMesh.RecalculateNormals();

            GameObject newGameObject = new GameObject("newGameObject");
            newGameObject.AddComponent<MeshRenderer>().material = meshRenderer.material;
            newGameObject.AddComponent<MeshFilter>().mesh = newMesh;
        }
        Destroy(this.gameObject);
    }
}
