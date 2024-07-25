using UnityEngine;

public class WoodSpawner : MonoBehaviour
{
    [SerializeField]private GameObject WoodPrefab;

    private void Awake() {
        int randomNum = Random.Range(0, 4);
        if (transform.CompareTag("RootSpawner"))
        {
            randomNum = Random.Range(0, 2);
            Debug.Log(randomNum);
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit))
            {
                transform.position = raycastHit.point;
            }
        }
        for (int i = 0; i < randomNum; i++)
        {
            Instantiate(WoodPrefab, transform);
        }
    }
} 

