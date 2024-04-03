using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private float timerDuration = 18.7f;
    private float timer;
    private void Start() {
        timer = timerDuration;
    }
    private void Update() {
        timer -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other) {
        if (transform.TryGetComponent(out AudioSource audioSource) && timer <= 0)
        {
            audioSource.Play();
        }
    }
}
