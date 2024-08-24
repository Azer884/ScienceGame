using UnityEngine;

public class SpawnPt : MonoBehaviour
{
    private Transform player;
    private bool tped;
    private void Start() {
        player = GameObject.Find("Player").transform;
        
    }
    private void Update() {
        if (!tped)
        {
            player.position = transform.position;
            tped = true;
        }
    }
}