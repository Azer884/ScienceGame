using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{

    private Rigidbody rb;
    private Transform GrabPoint;
    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Grab(Transform GrabPoint)
    {
        //rb.isKinematic = true;
        this.GrabPoint = GrabPoint;
        rb.useGravity = false;
        transform.parent = GrabPoint;

    }
    public void Drop()
    {
        //rb.isKinematic = false;
        GrabPoint = null;
        rb.useGravity = true;
        transform.parent = null;

    }
    private void FixedUpdate() 
    { 
        if (GrabPoint != null)
        {
            //rb.transform.rotation = Quaternion.Euler(0f,0f,0f);
            Vector3 targetPosition = GrabPoint.position;

            // Smoothly move the object towards the target position
            transform.position = targetPosition;
        }
    }
}