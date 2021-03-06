﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    public int width, height;

    private Vector3[] vertices;
    private Mesh mesh;

    private void Awake()
    {
        Generate();
    }
    
    private void Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(width + 1) * (height + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i=0, y= 0; y <= height; y++)
        {
            for(int x = 0; x <= width; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / width, (float)y / height);
                tangents[i] = tangent;
            }
        }
        mesh.vertices = vertices;

        int[] triangles = new int[width * height * 6];
        for(int ti = 0, vi = 0, y = 0; y < height; y++, vi++) {
            for (int x = 0; x < width; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + width + 1;
                triangles[ti + 5] = vi + width + 2;
            }
        }
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.tangents = tangents;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        Gizmos.color = Color.black;
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
