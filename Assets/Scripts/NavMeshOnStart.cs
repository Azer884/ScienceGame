using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshOnStart : MonoBehaviour
{
    private bool isFirstFrame = true;
    // Start is called before the first frame update
    void Update()
    {
        if (isFirstFrame)
        {
            NavMeshSurface surface = GetComponent<NavMeshSurface>();
            surface.BuildNavMesh();
            isFirstFrame = false;
        }
    }
}
