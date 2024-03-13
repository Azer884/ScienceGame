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
    [HideInInspector] public bool IsOnPos = false;
    [HideInInspector] public Transform TargetPosForNail;
    [HideInInspector]public float ContainerHeight;
    [HideInInspector]public bool IsConnected = false;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        OriginalParent = transform.parent;
        TargetPosForNail = transform;
    }

    public void Grab(Transform GrabPoint)
    {
        IsGrabbed = true;
        this.GrabPoint = GrabPoint;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
        IsOnPos = false;
        
        //transform.parent = OriginalParent;
    }
    public void Drop()
    {
        IsGrabbed = false;
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

                transform.position = Vector3.Lerp(transform.position, targetClip.position, Time.deltaTime * 60f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetClip.rotation, Time.deltaTime * 60f);
            }
        }

        if (IsOnPos) 
        {
            transform.localPosition = Vector3.up * ContainerHeight;
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        if (IsConnected)
        {
            Transform ConnectorTransform = GrabPoint.GetComponent<GrabSystem>().ConnectorPos;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            transform.position = ConnectorTransform.position;
            transform.rotation = ConnectorTransform.rotation;
            
        }
    }
}
