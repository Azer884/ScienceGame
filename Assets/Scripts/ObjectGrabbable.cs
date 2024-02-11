using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{

    private Rigidbody rb;
    private Transform GrabPoint;
    private Transform player;
    private Transform Cam;
    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    public void Grab(Transform GrabPoint)
    {
        //rb.isKinematic = true;
        this.GrabPoint = GrabPoint;
        rb.useGravity = false;
        transform.parent = GrabPoint;
        rb.velocity = Vector3.zero;

    }
    public void Drop()
    {
        //rb.isKinematic = false;
        GrabPoint = null;
        rb.useGravity = true;
        transform.parent = null;

    }
    private void Update() 
    { 
        if (GrabPoint != null)
        {
            Vector3 targetPosition = GrabPoint.position;
            

            transform.position = targetPosition;   
        }
    }
}
