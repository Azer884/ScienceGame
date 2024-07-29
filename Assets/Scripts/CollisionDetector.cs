using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private float timerDuration = 18.7f;
    private float timer;
    private AudioSource audioSource;
    private void Start() {
        timer = timerDuration;
        if (transform.TryGetComponent(out audioSource))
        {
            audioSource.playOnAwake = true;
            audioSource.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Trigger");
        }
    }
    private void Update() {
        timer -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other) {
        if (audioSource != null && timer <= 0)
        {
            audioSource.enabled = true;
            StartCoroutine(DisableGameObjectAfterDelay(audioSource, 1f));
        }
    }
    IEnumerator DisableGameObjectAfterDelay(AudioSource obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.enabled = false;
    }
}
