using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{

    private Rigidbody rb;
    [HideInInspector] public Transform GrabPoint;
    private Transform player;
    private Transform Cam;
    private Transform OriginalParent;
    [HideInInspector] public bool IsGrabbed = false; 
    [HideInInspector] public bool IsClipOnNail = false;
    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        OriginalParent = transform.parent;
    }

    public void Grab(Transform GrabPoint)
    {
        this.GrabPoint = GrabPoint;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        //transform.parent = GrabPoint;
    }
    public void Drop()
    {
        GrabPoint = null;
        rb.useGravity = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        //transform.parent = OriginalParent;
    }
    private void Update() 
    { 
        if (GrabPoint != null)
        {
            if (IsGrabbed)
            {
                transform.position = GrabPoint.position;
            }
        }
        if (IsClipOnNail)
        {   
            Transform[] targetBlackClips = GrabPoint.GetComponent<GrabSystem>().TargetBlackClips;
            IsGrabbed = false;

            Transform targetClip = targetBlackClips[0];
            if (targetClip != null)
            {
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeAll;

                // Set the position and rotation of the object smoothly
                transform.position = Vector3.Lerp(transform.position, targetClip.position, Time.deltaTime * 60f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetClip.rotation, Time.deltaTime * 60f);
            }
        }
    }
}
