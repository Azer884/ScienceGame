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
            audioSource.gameObject.SetActive(true);
            StartCoroutine(DisableGameObjectAfterDelay(audioSource.gameObject, .75f));
        }
    }
    IEnumerator DisableGameObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
