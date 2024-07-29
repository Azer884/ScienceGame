using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public LayerMask Layer;
    public Animator animator;
    private GameObject SpawnedObj;
    public Color[] color;
    public Color[] PowerColor;
    void Update()
    {
        if (SpawnedObj != null)
        {
            SpawnedObj.transform.GetChild(0).TryGetComponent(out animator);
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 2f, Layer))
        {
            Vector3 spawnPosition = hit.point;
            if (SpawnedObj == null)
            {
                SpawnedObj = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            }
        }
        else if(SpawnedObj != null)
        {
            if (animator != null)
            {
                animator.SetTrigger("End");
            }

            Destroy(SpawnedObj, 1f);
            SpawnedObj = null;
        }
    }
}
