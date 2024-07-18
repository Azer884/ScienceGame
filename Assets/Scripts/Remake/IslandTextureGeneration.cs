using UnityEngine;
using Unity.Mathematics;

public class IslandTextureGeneration : MonoBehaviour
{
    public int TextureSize;
    public float NoiseScale, IslandSize;
    [Range(1, 20)] public int NoiseOctaves;
    [Range(0, 99999999)] public int Seed;

    public float HeightMultiplier = 5f;

    // Privates
    private Color[] col;
    private Texture2D tex;
    private Mesh mesh;
    private Vector3[] vertices;

    public Gradient ColorGradient;

    private void Start()
    {
        tex = new Texture2D(TextureSize, TextureSize);
        col = new Color[tex.height * tex.width];

        Renderer rend = GetComponent<MeshRenderer>();
        rend.sharedMaterial.mainTexture = tex;

        Vector2 org = new Vector2(Mathf.Sqrt(Seed), Mathf.Sqrt(Seed));

        for (int x = 0, i = 0; x < TextureSize; x++)
        {
            for (int y = 0; y < TextureSize; y++, i++)
            {
                float noiseValue = Noisefunction(x, y, org);
                col[i] = ColorGradient.Evaluate(noiseValue);
            }
        }
        tex.SetPixels(col);
        tex.Apply();
        tex.wrapMode = TextureWrapMode.Clamp;

        GenerateMesh(org);
    }

    private float Noisefunction(float x, float y, Vector2 Origin)
    {
        float a = 0, noisesize = NoiseScale, opacity = 1;

        for (int octaves = 0; octaves < NoiseOctaves; octaves++)
        {
            float xVal = (x / (noisesize * TextureSize)) + Origin.x;
            float yVal = (y / (noisesize * TextureSize)) - Origin.y;
            float z = noise.snoise(new float2(xVal, yVal));
            a += Mathf.InverseLerp(-1, 1, z) / opacity;

            noisesize /= 2f;
            opacity *= 2f;
        }

        return Mathf.Clamp01(a - FallOffMap(x, y, TextureSize, IslandSize));
    }

    private float FallOffMap(float x, float y, int size, float islandSize)
    {
        float gradient = 1;

        gradient /= (x * y) / (size * size) * (1 - (x / size)) * (1 - (y / size));
        gradient -= 16;
        gradient /= islandSize;

        return gradient;
    }

    private void GenerateMesh(Vector2 origin)
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[TextureSize * TextureSize];
        int[] triangles = new int[(TextureSize - 1) * (TextureSize - 1) * 6];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int x = 0, i = 0; x < TextureSize; x++)
        {
            for (int y = 0; y < TextureSize; y++, i++)
            {
                float height = Noisefunction(x, y, origin) * HeightMultiplier;
                vertices[i] = new Vector3(x, height, y);
                uv[i] = new Vector2((float)x / TextureSize, (float)y / TextureSize);
            }
        }

        int vert = 0;
        int tris = 0;
        for (int x = 0; x < TextureSize - 1; x++)
        {
            for (int y = 0; y < TextureSize - 1; y++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + TextureSize + 0;
                triangles[tris + 2] = vert + TextureSize + 1;
                triangles[tris + 3] = vert + 0;
                triangles[tris + 4] = vert + TextureSize + 1;
                triangles[tris + 5] = vert + 1;

                vert++;
                tris += 6;
            }
            vert++;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }
}
