using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private TimeManager timeManager;
    public List<GameObject> flowers;
    public List<GameObject> roots;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }

    private void NightItemsSpawner(bool spawn)
    {
        foreach (GameObject flower in flowers)
        {
            flower.SetActive(spawn);
        }
        foreach (GameObject root in roots)
        {
            root.SetActive(spawn);
        }
    }
    private void Update() {

        if (timeManager.Hours >= 6 && timeManager.Hours <= 8)
        {
            NightItemsSpawner(false);
        }
        else if (timeManager.Hours == 18)
        {
            NightItemsSpawner(true);
        }
    }
}
