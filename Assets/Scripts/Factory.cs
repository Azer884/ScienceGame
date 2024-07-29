using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class Factory : MonoBehaviour
{
    private float[,] _heightMap;
    private float[,] _treeMap;

    [SerializeField] private SettingsSO settings;
    [SerializeField] private TreeSettingsSO treeSettings;

    [Space, SerializeField] private GameObject quad;
    [Space, SerializeField] private float overlapSphereRadius = 5f;

    private void Start()
    {
        if (settings != null && settings.GenerateOnStart)
        {
            Generate();
        }
    }

    private void Generate()
    {
        var size = settings.Size;
        var seed = settings.RandomizeSeed ? Random.Range(int.MinValue, int.MaxValue) : settings.Seed.GetHashCode();
        Random.InitState(seed);

        var falloffMap = new FalloffMap(size);
        var noiseMap = new NoiseMap(
            size,
            seed,
            settings.NoiseScale,
            settings.Octaves,
            settings.Persistance,
            settings.Lacunarity,
            settings.NoiseOffset);

        _heightMap = new float[size, size];
        _treeMap = new float[size, size];

        for (uint y = 0; y < settings.Size; y++)
        {
            for (uint x = 0; x < settings.Size; x++)
            {
                _heightMap[y, x] = noiseMap[y, x] - falloffMap[y, x];
                _treeMap[y, x] = Random.value;
            }
        }

        Draw();
        SpawnTrees();
    }

    private void Draw()
    {
        if (!quad)
        {
            return;
        }

        var size = _heightMap.GetLength(0);
        var mesh = new Mesh();
        var vertices = new Vector3[size * size];
        var uv = new Vector2[size * size];
        var triangles = new int[(size - 1) * (size - 1) * 6];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                int index = y * size + x;
                vertices[index] = new Vector3(x, _heightMap[y, x] * settings.Depth, y);
                uv[index] = new Vector2((float)x / size, (float)y / size);
            }
        }

        int triangleIndex = 0;
        for (int y = 0; y < size - 1; y++)
        {
            for (int x = 0; x < size - 1; x++)
            {
                int topLeft = y * size + x;
                int topRight = topLeft + 1;
                int bottomLeft = topLeft + size;
                int bottomRight = bottomLeft + 1;

                triangles[triangleIndex++] = topLeft;
                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = topRight;

                triangles[triangleIndex++] = topRight;
                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = bottomRight;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        quad.GetComponent<MeshFilter>().mesh = mesh;

        if (quad.TryGetComponent<MeshCollider>(out var meshCollider))
        {
            meshCollider.sharedMesh = mesh;
        }
    }

    private void SpawnTrees()
    {
        int size = _heightMap.GetLength(0);
        List<Vector3> treePositions = new();

        int playerSpawnnPt = Random.Range(size/2 - 9, size/2 + 9);
        while (_treeMap[playerSpawnnPt, playerSpawnnPt] < 0.001f)
        {
            playerSpawnnPt = Random.Range(size/2 - 20, size/2 + 20);
        }
        Vector3 playerPosition = new(playerSpawnnPt * 2.3612f, _heightMap[playerSpawnnPt, playerSpawnnPt] * settings.Depth * 2.3612f, playerSpawnnPt * 2.3612f);
        Quaternion playerRot = treeSettings.treePrefabs[27].transform.rotation;
        GameObject player = Spawn(treeSettings.treePrefabs[27], playerPosition,playerRot , transform, 27);
        player.hideFlags = HideFlags.DontSave;
        
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (_heightMap[y, x] >= treeSettings.minHeight && _heightMap[y, x] <= treeSettings.maxHeight && _treeMap[y, x] >= .988f)
                {
                    int treeIndex = Random.Range(0, treeSettings.treePrefabs.Length - 2);

                    if (_treeMap[y, x] < treeSettings.density && _treeMap[y, x] >= .989f)
                    {
                        treeIndex = 24; // Flowers
                    }
                    else if (_treeMap[y, x] < .989f && _treeMap[y, x] >= .988f)
                    {
                        treeIndex = 25; // Berries
                    }

                    Vector3 treePosition = new(x * 2.3612f, _heightMap[y, x] * settings.Depth * 2.3612f - .5f, y * 2.3612f);
                    
                    if (!IsOverlapping(treePosition, treePositions))
                    {
                        Quaternion treeRot = treeSettings.treePrefabs[treeIndex].transform.rotation;
                        GameObject tree = Spawn(treeSettings.treePrefabs[treeIndex], treePosition, treeRot, transform, treeIndex);
                        tree.hideFlags = HideFlags.DontSave;
                        treePositions.Add(treePosition);
                    }
                }
                else if (_heightMap[y, x] < treeSettings.minHeight && _treeMap[y, x] > .9995f)
                {
                    Vector3 obsidianPosition = new(x * 2.3612f, _heightMap[y, x] * settings.Depth * 2.3612f - .5f, y * 2.3612f);
                    
                    if (!IsOverlapping(obsidianPosition, treePositions))
                    {
                        Quaternion obsidianRot = treeSettings.treePrefabs[26].transform.rotation;
                        GameObject obsidian = Spawn(treeSettings.treePrefabs[26], obsidianPosition, obsidianRot, transform, 26);
                        obsidian.hideFlags = HideFlags.DontSave;
                        treePositions.Add(obsidianPosition);
                    }
                }
            }
        }
        
            
            
        
    }

    private bool IsOverlapping(Vector3 position, List<Vector3> positions)
    {
        foreach (var pos in positions)
        {
            if (Vector3.Distance(position, pos) < overlapSphereRadius)
            {
                return true;
            }
        }
        return false;
    }


    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private GameObject Spawn(GameObject obj, Vector3 pos, Quaternion rot, Transform parent, int index)
    {
        GameObject tree = Instantiate(obj, pos, rot, parent);
        if (new[] {24, 25, 26, 27}.Contains(index))
        {
            
        }
        return tree;
    }
}
