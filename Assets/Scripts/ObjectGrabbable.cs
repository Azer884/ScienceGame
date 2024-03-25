using System.Collections;
using LiquidVolumeFX;
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
    [HideInInspector]public Transform targetClip;
    [HideInInspector]public bool Electricity;
    [HideInInspector] public bool IsOnPos = false;
    [HideInInspector] public Transform TargetPosForNail;
    [HideInInspector]public float ContainerHeight;
    [HideInInspector]public bool IsConnected = false;

    [HideInInspector]public bool CountdownCheck = false;
    [HideInInspector]public float targetPoint;
    private Coroutine countdownCoroutine;
    [HideInInspector]public float targetTime;

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
        Electricity = false;
        
        transform.parent = OriginalParent;
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
            IsGrabbed = false;
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

            if(gameObject.layer == LayerMask.NameToLayer("Nail"))
            {
                if (transform.parent.parent.parent.GetComponentInChildren<LiquidVolume>().liquidLayers[1].amount >= .1f && UiManager.ColorCheck(transform.parent.parent.parent.GetComponentInChildren<LiquidVolume>().liquidLayers[1].color, FindAnyObjectByType<UiManager>().color[0]))
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
            else if(gameObject.layer == LayerMask.NameToLayer("BronzePlate"))
            {
                if (transform.parent.parent.parent.GetComponentInChildren<LiquidVolume>().liquidLayers[1].amount >= .1f && UiManager.ColorCheck(transform.parent.parent.parent.GetComponentInChildren<LiquidVolume>().liquidLayers[1].color, FindAnyObjectByType<UiManager>().color[3]))
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

        if (IsConnected)
        {
            Transform ConnectorTransform = GrabPoint.GetComponent<GameManager>().ConnectorPos;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;

            Electricity = true;
            transform.position = ConnectorTransform.position;
            transform.rotation = ConnectorTransform.rotation;
            IsConnected = false;
            
        }

        if (CountdownCheck)
        {
            if(gameObject.layer == LayerMask.NameToLayer("Nail"))
            {
                ChangeColor(transform.parent.parent.parent.GetComponentInChildren<LiquidVolume>(), FindAnyObjectByType<UiManager>().color[1]);
            }
            else if (gameObject.layer == LayerMask.NameToLayer("BronzePlate"))
            {
                ChangeColor(transform.parent.parent.parent.GetComponentInChildren<LiquidVolume>(), FindAnyObjectByType<UiManager>().color[4]);
            }
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
    private void ChangeColor(LiquidVolume lv, Color color)
    {
        lv.liquidLayers[1].color = Color.Lerp(lv.liquidLayers[1].color ,color, targetPoint);
        lv.UpdateLayers(true);
        targetPoint += Time.deltaTime * .0001f;
        if (lv.liquidLayers[1].color == color)
        {
            CountdownCheck = false;
            targetPoint = 0f;
        }
    }
}