using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightObj : MonoBehaviour
{
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    [System.Obsolete]
    void Awake()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        
        spawnManager.flowers.Add(gameObject);
    }
}
