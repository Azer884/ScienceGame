using UnityEngine;

namespace _Project.Map.Scripts
{
    [ExecuteAlways]
    internal class Factory : MonoBehaviour
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
                    _treeMap[y, x] = Random.Range(0f, 1f);
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
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (_heightMap[y, x] >= treeSettings.minHeight && _heightMap[y, x] <= treeSettings.maxHeight && _treeMap[y, x] >= .988f)
                    {
                        int TreeIndex = Random.Range(0,treeSettings.treePrefabs.Length - 1);
                        if (_treeMap[y, x] < treeSettings.density && _treeMap[y, x] >= .989f)
                        {
                            TreeIndex = 24;
                        }
                        else if (_treeMap[y, x] < .989f && _treeMap[y, x] >= .988f)
                        {
                            TreeIndex = 25;
                        }

                        Vector3 treePosition = new(x * 2.3612f, _heightMap[y, x] * settings.Depth * 2.3612f - .5f, y * 2.3612f);
                        Quaternion treeRot = treeSettings.treePrefabs[TreeIndex].transform.rotation;
                        GameObject tree = Instantiate(treeSettings.treePrefabs[TreeIndex], treePosition, treeRot, transform);
                        tree.hideFlags = HideFlags.DontSave;
                    }
                    else if (_heightMap[y, x] < treeSettings.minHeight && _treeMap[y, x] > .9995f)
                    {
                        Vector3 obsidianPosition = new(x * 2.3612f, _heightMap[y, x] * settings.Depth * 2.3612f - .5f, y * 2.3612f);
                        Quaternion ObsidianRot = treeSettings.treePrefabs[26].transform.rotation;
                        GameObject obsidian = Instantiate(treeSettings.treePrefabs[26], obsidianPosition, ObsidianRot, transform);
                        obsidian.hideFlags = HideFlags.DontSave;
                    }
                }
            }
            RemoveOverlappingTrees();
        }

        private void RemoveOverlappingTrees()
        {
            foreach (Transform tree in transform)
            {
                Collider[] colliders = Physics.OverlapSphere(tree.position, overlapSphereRadius);
                foreach (Collider collider in colliders)
                {
                    if (collider.transform != tree && collider.CompareTag("Tree"))
                    {
                        DestroyImmediate(collider.gameObject);
                    }
                }
            }
        }

        private void OnDisable()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
