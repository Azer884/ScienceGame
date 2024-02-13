using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes.Test;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    [SerializeField] private Transform PlayerCam;
    [SerializeField] private Transform GrabPiont;
    [SerializeField] private LayerMask PickupLayer;
    [SerializeField] private LayerMask OutLineLayer;
    private ObjectGrabbable Object;
    private float xRot;
    public Transform armTarget;

    private float rotationSpeed = 5f;
    private float targetXRot = 0f; // Target value for xRot

    public Outline LongNail;
    public Outline ShortNail;
    public Transform[] TargetRedClips;
    public Transform[] TargetBlackClips;

    private int i = 0;

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if(Object == null)
            {
                float pickUpDistance = 2f;
                if (Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit raycastHit, pickUpDistance, PickupLayer))
                {
                    if (raycastHit.transform.TryGetComponent(out Object))
                    {
                        Object.Grab(GrabPiont);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(Object != null)
            {
                Object.Drop();
            }
            Object = null;
            
        }

        SelectNail();

        if (Object != null)
        {
            Object.transform.rotation = armTarget.rotation;
        }
        if(transform.childCount != 0)
        {
            if(transform.GetChild(0).CompareTag("BlackClip") || transform.GetChild(0).CompareTag("RedClip")) return;
        }

        targetXRot = Input.GetMouseButton(1) ? 85f : 0f;
        xRot = Mathf.Lerp(xRot, targetXRot, Time.deltaTime * rotationSpeed);

        armTarget.localRotation = Quaternion.Euler(0f, 0f, xRot);

        
    }


    void SelectNail()
    {
        if(transform.childCount != 0)

        {
            if (transform.GetChild(0).CompareTag("BlackClip"))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _, 2f, OutLineLayer))
                {

                    LongNail.enabled = true;
                    
                    /*if (Input.GetMouseButton(1))
                    {
                        transform.GetChild(0).transform.SetPositionAndRotation(TargetBlackClips[i].position, TargetBlackClips[i].rotation);
                        i++;
                    }*/

                }
                else
                {
                    LongNail.enabled = false;
                }
            }
            else if (transform.GetChild(0).CompareTag("RedClip"))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _, 2f, OutLineLayer))
                {

                    ShortNail.enabled = true;

                    /*if (Input.GetMouseButton(1))
                    {
                        transform.GetChild(0).transform.SetPositionAndRotation(TargetBlackClips[i].position, TargetBlackClips[i].rotation);
                        i++;
                    }*/

                }
                else
                {
                    ShortNail.enabled = false;
                }
            }
        }
        else
        {
            LongNail.enabled = false;
            ShortNail.enabled = false;
        }
    }
}
