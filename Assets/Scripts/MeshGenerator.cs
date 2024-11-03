using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class MeshGenerator
{
    public static void CreatePlaneMesh(int n, Material planeMaterial)
    {
        Debug.Log("CreatePlane: Starting to create a plane with size: " + n);
        GameObject plane = new GameObject("HeightMapPlane");
        MeshFilter meshFilter = plane.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = plane.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();
        plane.transform.position = Vector3.zero;
        Debug.Log("CreatePlane: Plane position set to " + plane.transform.position);

        Vector3[] vertices = new Vector3[n * n];
        Vector2[] uv = new Vector2[n * n];
        int[] triangles = new int[(n - 1) * (n - 1) * 6];
        Debug.Log("CreatePlane: Mesh arrays initialized.");

        for (int z = 0, i = 0; z < n; z++)
        {
            for (int x = 0; x < n; x++, i++)
            {
                vertices[i] = new Vector3(x , 0, z ); // Flat plane at y = 0
                uv[i] = new Vector2((float)x / (n - 1), (float)z / (n - 1)); // UV mapping
                
                if (x < n - 1 && z < n - 1)
                {
                    int start = i; // Current vertex index
                    
                    // Set the triangle indices correctly
                    triangles[(z * (n - 1) + x) * 6 + 0] = start;
                    triangles[(z * (n - 1) + x) * 6 + 1] = start + n;
                    triangles[(z * (n - 1) + x) * 6 + 2] = start + 1;

                    triangles[(z * (n - 1) + x) * 6 + 3] = start + 1;
                    triangles[(z * (n - 1) + x) * 6 + 4] = start + n;
                    triangles[(z * (n - 1) + x) * 6 + 5] = start + n + 1;
                }
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        Debug.Log("CreatePlane: Mesh created with " + vertices.Length + " vertices and " + triangles.Length + " triangles.");

        meshFilter.mesh = mesh;

        if (planeMaterial != null)
        {
            meshRenderer.material = planeMaterial;
            Debug.Log("CreatePlane: Material assigned to the mesh.");
        }
        else
        {
            Debug.LogError("CreatePlane: Plane material is not assigned!");
        }

        Debug.Log("CreatePlane: Plane added to the scene.");

        
    }
}