using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    [SerializeField] private Transform PlayerCam;
    [SerializeField] private LayerMask PickupLayer;
    [SerializeField] private LayerMask OutLineLayer;
    private ObjectGrabbable Object;
    private float xRot;
    public Transform armTarget;

    private readonly float rotationSpeed = 5f;
    private float targetXRot = 0f;

    public Outline LongNail;
    public Outline ShortNail;
    private Transform Clip;
    public Transform[] TargetRedClips;
    public Transform[] TargetBlackClips;
    [HideInInspector]public bool IsNail = false;



    private void Update()
    {
        SelectNail();
        if (IsNail)
        {
            Object = null;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(Object == null)
            {
                float pickUpDistance = 2f;
                if (Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit raycastHit, pickUpDistance, PickupLayer))
                {
                    if (raycastHit.transform.TryGetComponent(out Object) && !IsNail)
                    {
                        Object.Grab(transform);
                    }
                }
            }
        }
            
        else if (Input.GetMouseButtonUp(0) && !IsNail)
        {
            if(Object != null)
            {
                Object.Drop();
            }
            Object = null;
        }

        if (Object != null && !IsNail)
        {
            Object.transform.rotation = armTarget.rotation;
        }

        if(!IsNail)
        {
            targetXRot = Input.GetMouseButton(1) ? 85f : 0f;
            xRot = Mathf.Lerp(xRot, targetXRot, Time.deltaTime * rotationSpeed);

            armTarget.localRotation = Quaternion.Euler(0f, 0f, xRot);
        }
    }


    public void SelectNail()
    {
        if(transform.childCount != 0)

        {
            if (transform.GetChild(0).CompareTag("BlackClip"))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _, 2f, OutLineLayer))
                {
                    LongNail.enabled = true;
                    Clip = transform.GetChild(0);
                    if(Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        IsNail = true;
                        Clip.SetParent(TargetBlackClips[0]);
                    }
                    else
                    {
                        IsNail = false;
                    }
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

                    Clip = transform.GetChild(0);
                    if(Input.GetKeyDown(KeyCode.Mouse1))
                    {       
                        IsNail = true;
                        Clip.SetParent(TargetBlackClips[0]);
                    }
                    else
                    {
                        IsNail = false;
                    }
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
            IsNail = false;
        }
    }
}
