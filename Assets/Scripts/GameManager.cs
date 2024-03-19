using System.Collections;
using UnityEngine;
using LiquidVolumeFX;

public class GameManager : MonoBehaviour
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
    public Outline ShortNailOutLine;
    public Connector[] ShortNail;
    public Transform TargetClipPos;
    [HideInInspector]public bool IsInteractable = false;
    [HideInInspector]public Outline TubeOutLine;

    private float targetTime;
    private Coroutine countdownCoroutine;
    private Transform Flask;
    private float targetPoint;
    [HideInInspector]public bool CountdownCheck = false;
    public Color color;
    public UiManager UI;
    public Transform ConnectorPos;
    private Connector connector;
    public LayerMask ConnectorLayer;

    public LayerMask BarrelLayer;
    private Outline BarrelOutLine;

    public LayerMask SinkLayer;
    private Outline Sinkoutline;


    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if(Object == null)
            {
                float pickUpDistance = 2f;
                if (Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit raycastHit, pickUpDistance, PickupLayer))
                {
                    if (raycastHit.transform.TryGetComponent(out Object) && !IsInteractable)
                    {
                        Object.Grab(transform);
                        Object.IsClipOnNail = false;
                        Object.IsConnected = false;
                        if (connector != null && (connector.ConnectedWire == Object.transform))
                        {
                            connector.ConnectedWire = null;
                        }
                        connector = null;
                        if (ShortNail[0] != null && (ShortNail[0].ConnectedWire == Object.transform))
                        {
                            ShortNail[0].ConnectedWire = null;
                        }
                        ShortNail[0] = null;
                        if (ShortNail[1] != null && (ShortNail[1].ConnectedWire == Object.transform))
                        {
                            ShortNail[1].ConnectedWire = null;
                        }
                        ShortNail[1] = null;

                    }
                }
            }
        }
            
        else if (Input.GetMouseButtonUp(0) && !IsInteractable)
        {
            if(Object != null)
            {
                Object.Drop();
                Object.IsGrabbed = false;
                Object.IsClipOnNail = false;
            }
            Object = null;
        }

        SetObjOnPosition();
        
        if (IsInteractable)
        {
            Object = null;
        }

        if (Object != null && !IsInteractable)
        {
            Object.transform.rotation = armTarget.rotation;
        }

        if(Object == null && Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit sink, 2f, SinkLayer))
        {
            if(sink.transform.name == "SinkTap")
            {
                if (sink.transform.TryGetComponent(out Sinkoutline))
                {
                    Sinkoutline.enabled = true;

                    if (Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        if (sink.transform.TryGetComponent(out Sink sink1))
                        {
                            if(!sink1.IsOpened)
                            {
                                sink1.IsOpened = true;
                            }
                            else
                            {
                                sink1.IsOpened = false;
                            }
                        }
                        Sinkoutline.enabled = false;
                    }
                }
            }
            else if(Sinkoutline != null)
            {
                Sinkoutline.enabled = false;
            }
        }
        else if(Sinkoutline != null)
        {
            Sinkoutline.enabled = false;
        }
        
        else if(Object == null || (Object.gameObject.layer == LayerMask.NameToLayer("Default") && !IsInteractable))
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

                        if (Flask.GetComponentInChildren<PouringSystem>().liquid.liquidLayers[1].amount >= .1f && UiManager.ColorCheck(Flask.GetComponentInChildren<PouringSystem>().liquid.liquidLayers[1].color, UI.color[0]))
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


    public void SetObjOnPosition()
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
                        IsInteractable = true;
                        Object.IsClipOnNail = true;
                    }
                    else
                    {
                        IsInteractable = false;
                    }
                }
                else
                {
                    LongNail.enabled = false;
                }
            }

            else if (Object.CompareTag("RedClip"))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 2f, OutLineLayer))
                {
                    ShortNailOutLine.enabled = true;
                    if(raycastHit.transform.GetChild(2).TryGetComponent(out ShortNail[0]) && raycastHit.transform.GetChild(3).TryGetComponent(out ShortNail[1]))
                    {
                        if (!ShortNail[0].Connected)
                        {
                            if(Input.GetKeyDown(KeyCode.Mouse1))
                            {       
                                IsInteractable = true;
                                Object.IsClipOnNail = true;
                                ShortNail[0].ConnectedWire = Object.transform;
                                TargetClipPos = ShortNail[0].transform;
                                ShortNailOutLine.enabled = false;
                            }
                            else
                            {
                                IsInteractable = false;
                            }
                        }
                        else if (!ShortNail[1].Connected)
                        {
                            if(Input.GetKeyDown(KeyCode.Mouse1))
                            {       
                                IsInteractable = true;
                                Object.IsClipOnNail = true;
                                ShortNail[1].ConnectedWire = Object.transform;
                                TargetClipPos = ShortNail[1].transform;
                                ShortNailOutLine.enabled = false;
                            }
                            else
                            {
                                IsInteractable = false;
                            }
                        }
                    }
                }
                else
                {
                    ShortNailOutLine.enabled = false;
                }
                
            }

            else if (Object.CompareTag("MaleConnector"))
            {

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 2f, ConnectorLayer))
                {

                    if(raycastHit.transform.TryGetComponent(out connector))
                    {
                        if (!connector.Connected)
                        {
                            if(Input.GetKeyDown(KeyCode.Mouse1))
                            {
                                connector.ConnectedWire = Object.transform;

                                ConnectorPos = connector.transform.GetChild(1);
                                Object.IsGrabbed = false;
                                Object.IsConnected = true;
                                IsInteractable = true;
                                connector.GetComponent<Outline>().enabled = false;
                            }
                            connector.GetComponent<Outline>().enabled = true;
                        }
                        else
                        {
                            connector.GetComponent<Outline>().enabled = false;
                        }
                    }
                    else
                    {
                        IsInteractable = false;
                    }
                }
                else if(connector != null)
                {
                    connector.GetComponent<Outline>().enabled = false;
                }
            }
            else if (Object.transform.childCount > 1 && Object.transform.GetChild(1).TryGetComponent(out LiquidVolume lv))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 2f, BarrelLayer))
                {
                    BarrelOutLine = null;
                    if(raycastHit.transform.TryGetComponent(out BarrelOutLine))
                    {
                        BarrelOutLine.enabled = true;

                        if (Input.GetKeyDown(KeyCode.Mouse1))
                        {
                            if(CheckChildLiquidAmount(BarrelOutLine.transform, 1))
                            {
                                if(UiManager.ColorCheck(lv.liquidLayers[1].color, BarrelOutLine.GetComponent<BarrelLiquidColor>().color))
                                {
                                    lv.liquidLayers[1].amount += 0.2f;
                                    lv.UpdateLayers(true);
                                    BarrelOutLine.enabled = false;
                                }
                                else if (lv.liquidLayers[1].amount == 0f)
                                {
                                    lv.liquidLayers[1].color = BarrelOutLine.GetComponent<BarrelLiquidColor>().color;
                                    lv.liquidLayers[1].amount += 0.2f;
                                    lv.UpdateLayers(true);
                                    BarrelOutLine.enabled = false;
                                }
                            }
                            if (Object.name == "MetalContainer")
                            {
                                
                            }
                        }
                    }
                }
                else if(BarrelOutLine != null)
                {
                    BarrelOutLine.enabled = false;
                }
            }
        }
        else
        {
            LongNail.enabled = false;
            ShortNailOutLine.enabled = false;
            IsInteractable = false;
            if (connector != null)
            {
                connector.GetComponent<Outline>().enabled = false;
            }
            if (BarrelOutLine != null)
            {
                BarrelOutLine.enabled = false;
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

    bool CheckChildLiquidAmount(Transform parent, int i)
    {
        bool v = false;
        for (int child = 0; child < parent.childCount; child++)
        {
            if(parent.GetChild(child).TryGetComponent(out LiquidVolume lv))
            {
                if (lv.liquidLayers[i].amount > 0.001f)
                {
                    v = true;
                }
                break;
            }
        }
        return v;
    }
}