using UnityEngine;

[CreateAssetMenu(fileName = "TreeSettings", menuName = "Map/TreeSettings")]
public class TreeSettingsSO : ScriptableObject
{
    public GameObject[] treePrefabs;
    public float minHeight; // Minimum height for tree placement
    public float maxHeight; // Maximum height for tree placement
    public float density; // Density of trees
}
