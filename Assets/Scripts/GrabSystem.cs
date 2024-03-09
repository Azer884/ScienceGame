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
    public Transform[] TargetRedClips;
    public Transform[] TargetBlackClips;
    [HideInInspector]public bool IsNail = false;
    [HideInInspector]public Outline TubeOutLine;

    private float targetTime;
    private Coroutine countdownCoroutine;
    private Transform Flask;
    private float targetPoint;
    private bool CountdownCheck = false;
    public Color color;



    private void Update()
    {

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
                        Object.IsClipOnNail = false;

                    }
                }
            }
        }
            
        else if (Input.GetMouseButtonUp(0) && !IsNail)
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
        
        if (IsNail)
        {
            Object = null;
        }

        if (Object != null && !IsNail)
        {
            Object.transform.rotation = armTarget.rotation;
        }

        if(Object == null || (Object.gameObject.layer == LayerMask.NameToLayer("Default") && !IsNail))
        {
            targetXRot = Input.GetMouseButton(1) ? 85f : 0f;
            xRot = Mathf.Lerp(xRot, targetXRot, Time.deltaTime * rotationSpeed);

            armTarget.localRotation = Quaternion.Euler(0f, 0f, xRot);
        }

        else if(Object.gameObject.layer == LayerMask.NameToLayer("Nail"))
        {
            CountdownCheck = false;
            targetPoint = 0f;
            targetTime = 10f;

            float pickUpDistance = 2f;
            if (Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit raycastHit, pickUpDistance, LayerMask.NameToLayer("NailLayer")))
            {
                if (raycastHit.transform.TryGetComponent(out TubeOutLine))
                {
                    TubeOutLine.enabled = true;
                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        Flask = raycastHit.transform;
                        
                        switch (raycastHit.transform.name)
                        {
                            case "TestTube":
                                Object.ContainerHeight = 0.013f;
                            break;

                            case "Beaker":
                                Object.ContainerHeight = 0.005f;
                            break;

                            case "Erlenmeyer":
                                Object.ContainerHeight = 0.04f;
                            break;

                            case "FlorenceFlask":
                                Object.ContainerHeight = 0.02f;
                            break;

                            case "Electrolysis":
                                Object.ContainerHeight = 0.07f;
                            break;
                        }

                        Object.TargetPosForNail = raycastHit.transform.GetChild(0).GetChild(0);
                        Object.transform.SetParent(raycastHit.transform.GetChild(0).GetChild(0));
                        Object.IsGrabbed = false;
                        Object.GrabPoint = null;
                        Object.IsOnPos = true;

                        if (Flask.GetComponentInChildren<PouringSystem>().liquid.liquidLayers[1].amount >= .1f)
                        {
                            countdownCoroutine ??= StartCoroutine(Countdown());
                        }
                        else
                        {
                            if(countdownCoroutine != null)
                            {
                                StopCoroutine(countdownCoroutine);
                                countdownCoroutine = null;
                                targetTime = 10f;
                            }
                        }

                    }
                }
            }
        }

        if (Object != null && ((Object.gameObject.layer != LayerMask.NameToLayer("Nail")) || !Physics.Raycast(PlayerCam.position, PlayerCam.forward, out _, 2f, LayerMask.NameToLayer("NailLayer"))) && TubeOutLine != null)
        {
            TubeOutLine.enabled = false;
        }
        else if (Object == null && TubeOutLine != null)
        {
            TubeOutLine.enabled = false;
        }

        if (CountdownCheck)
        {
            Flask.GetComponentInChildren<LiquidVolume>().liquidLayers[1].color = Color.Lerp(Flask.GetComponentInChildren<LiquidVolume>().liquidLayers[1].color ,color, targetPoint);
            Flask.GetComponentInChildren<LiquidVolume>().UpdateLayers(true);
            targetPoint += Time.deltaTime * .0001f;
            if (Flask.GetComponentInChildren<LiquidVolume>().liquidLayers[1].color == color)
            {
                CountdownCheck = false;
                targetPoint = 0f;
            }
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

    private IEnumerator Countdown()
    {
        targetTime = 10f;
        while (targetTime > 0)
        {
            yield return new WaitForSeconds(1);
            targetTime--;
        }
        CountdownCheck = true;

        
        

        countdownCoroutine = null;
    }
}
