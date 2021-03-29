using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public int depth = 20;
    public int octaves = 4;
    public float persistence = .5f;
    public float lacunarity = 2f;

    public float scale = 20f;

    public float offetX = 100f;
    public float offety = 100f;

    private void Start()
    {
        offetX = Random.Range(0f, 99999f);
        offety = Random.Range(0f, 99999f);
    }

    void Update()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        //offetX += Time.deltaTime * 5f;
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHights());
        return terrainData;
    }

    float[,] GenerateHights()
    {

        float[,] heights = new float[width, height];
        for(int x = 0; x< width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                float xCoord = (float)(x / width * scale + offetX);
                float yCoord = (float)(y / height * scale + offety) ;
                float perlin = Mathf.PerlinNoise(xCoord , yCoord);
                heights[x, y] = perlin;
            }
        }
        return heights;
    }

/*    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offetX;
        float yCoord = (float)y / height * scale + offety;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }*/
}
