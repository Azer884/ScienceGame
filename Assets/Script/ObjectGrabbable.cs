using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{

    private Rigidbody rb;
    private Transform GrabPoint;
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void Grab(Transform GrabPoint)
    {
        this.GrabPoint = GrabPoint;
        rb.useGravity = false;
        transform.parent = GrabPoint;
    }
    public void Drop()
    {
        this.GrabPoint = null;
        rb.useGravity = true;
        transform.parent = null;
    }
    private void FixedUpdate() 
    {
        if (GrabPoint != null)
        {
            float lerpSpeed = 10f;
            Vector3 targetPosition = GrabPoint.position;

            // Smoothly move the object towards the target position
            rb.MovePosition(Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed));
        }
    }
}
