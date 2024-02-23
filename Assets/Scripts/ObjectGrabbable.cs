using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{

    private Rigidbody rb;
    [HideInInspector] public Transform GrabPoint;
    private Transform player;
    private Transform Cam;
    private Transform OriginalParent;
    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        OriginalParent = transform.parent;
    }

    public void Grab(Transform GrabPoint)
    {
        rb.constraints = RigidbodyConstraints.None;
        this.GrabPoint = GrabPoint;
        rb.useGravity = false;
        transform.parent = GrabPoint;
        rb.velocity = Vector3.zero;
    }
    public void Drop()
    {
        rb.constraints = RigidbodyConstraints.None;
        GrabPoint = null;
        rb.useGravity = true;
        transform.parent = OriginalParent;
    }
    private void Update() 
    { 
        if (GrabPoint != null)
        {
            if (!GrabPoint.GetComponent<GrabSystem>().IsNail)
            {
                transform.position = GrabPoint.position;
            }
        }
        if (transform.parent.CompareTag("ClipPos"))
        {
            transform.SetLocalPositionAndRotation(Vector3.zero , Quaternion.Euler(0f, 0f, 0f));
        }
    }
}
