using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using LiquidVolumeFX;

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
    private Outline NailPlace;
    public Transform[] TargetRedClips;
    public Transform[] TargetBlackClips;
    [HideInInspector]public bool IsNail = false;
    public bool IsNailInPos = false;

    private LiquidVolume lv;



    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if(Object == null)
            {
                float pickUpDistance = 2f;
                if (Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit raycastHit, pickUpDistance, PickupLayer))
                {
                    if (raycastHit.transform.TryGetComponent(out Object) && !IsNail && !IsNailInPos)
                    {
                        Object.Grab(transform);
                        Object.IsGrabbed = true;
                        Object.IsClipOnNail = false;

                    }
                }
            }
        }
            
        else if (Input.GetMouseButtonUp(0) && !IsNail && !IsNailInPos)
        {
            if(Object != null)
            {
                Object.Drop();
                Object.IsGrabbed = false;
                Object.IsClipOnNail = false;
            }
            Object = null;
        }

        SelectNail();
        NailExp();
        
        if (IsNail && IsNailInPos)
        {
            Object = null;
        }

        if (Object != null && !IsNail && !IsNailInPos)
        {
            Object.transform.rotation = armTarget.rotation;
        }

        if(!IsNail && !IsNailInPos)
        {
            targetXRot = Input.GetMouseButton(1) ? 85f : 0f;
            xRot = Mathf.Lerp(xRot, targetXRot, Time.deltaTime * rotationSpeed);

            armTarget.localRotation = Quaternion.Euler(0f, 0f, xRot);
        }
    }


    public void SelectNail()
    {
        if(Object != null && Object.IsGrabbed)
        {
            if (Object.CompareTag("BlackClip"))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _, 2f, OutLineLayer))
                {
                    LongNail.enabled = true;
                    if(Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        IsNail = true;
                        Object.IsClipOnNail = true;
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

            else if (Object.CompareTag("RedClip"))
            {

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out _, 2f, OutLineLayer))
                {
                    ShortNail.enabled = true;

                    if(Input.GetKeyDown(KeyCode.Mouse1))
                    {       
                        IsNail = true;
                        Object.IsClipOnNail = true;
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
    void NailExp()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit place, 2f, OutLineLayer))
        {
            NailPlace = place.transform.GetComponent<Outline>();


            if(Object != null && Object.IsGrabbed)
            {
                if (Object.CompareTag("Nail"))
                {
                    NailPlace.enabled = true;
                    if(Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        Object.IsNailOn = true;
                        IsNailInPos = true;
                        Object.transform.SetParent(place.transform);
                    }
                    else
                    {
                        IsNailInPos = false;
                    }

                }

            }
            else
            {
                IsNailInPos = false;
            }
        
        }
        else if (NailPlace != null)
        {
            NailPlace.enabled = false;
        }
            
    }
}
