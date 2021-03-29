using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    Color[] colors;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    public int octaves = 1;
    public float persistence = .5f;
    public float lacunarity = 2f;

    public float scale = 20f;

    public float offetX = 100f;
    public float offety = 100f;

    public Gradient gradient;

    float minTerrHeight;
    float maxTerrHeight;

    void Start()
    {
        offetX = Random.Range(0f, 99999f);
        offety = Random.Range(0f, 99999f);

    }

    void Update()
    {

        /*offetX = Random.Range(0f, 99999f);
        offety = Random.Range(0f, 99999f);*/

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        float[,] heights = GenerateHights();

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = heights[z, x];
                vertices[i] = new Vector3(x, y, z);

                if (y > maxTerrHeight)
                    maxTerrHeight = y;
                if (y < minTerrHeight)
                    minTerrHeight = y;

                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float h = Mathf.InverseLerp(minTerrHeight,maxTerrHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(h);
                i++;
            }
        }

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
        
    }

    float[,] GenerateHights()
    {
        float[,] noiseMap = new float[xSize + 1, zSize + 1];

        float halfw = (float) xSize / 2;
        float halfh = (float) zSize / 2;

        for (int y = 0; y < zSize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeights = 0;
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x-halfw) / scale * frequency + offetX;
                    float sampleY = (y-halfh) / scale * frequency + offety;

                    float perlin = Mathf.PerlinNoise(sampleX, sampleY);
                    noiseHeights += perlin ;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }
                /*if (noiseHeights > maxNoise)
                {
                    maxNoise = noiseHeights;
                }
                else  if (noiseHeights < minNoise)
                {
                    minNoise = noiseHeights;
                }*/

                noiseMap[x, y] = noiseHeights * 8f;
            }
        }
        /*for (int y = 0; y < zSize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoise, noiseMap[x, y]);
            }
        }*/
        return noiseMap;
    }

    /*    private void OnDrawGizmos()
        {
            if (vertices == null)
                return;

            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], .1f);
            }
        }*/
}
