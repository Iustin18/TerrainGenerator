using UnityEngine;

public class Noise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

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
        Renderer render = GetComponent<Renderer>();
        render.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for(int x=0; x < width; x++)
        {
            for(int y = 0; y< height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    Color CalculateColor( int x, int y)
    {
        float xCoord = (float) x / width * scale + offetX;
        float yCoord = (float) y / height * scale + offety;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}
