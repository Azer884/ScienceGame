using System.Collections;
using UnityEngine;
using LiquidVolumeFX;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform PlayerCam;
    [SerializeField] private LayerMask PickupLayer;
    [SerializeField] private LayerMask OutLineLayer;
    [HideInInspector]public ObjectGrabbable Object;
    private float xRot;
    public Transform armTarget;

    private readonly float rotationSpeed = 5f;
    private float targetXRot = 0f;

    public Outline LongNailOutLine;
    public Outline ShortNailOutLine;
    public Transform[] TargetClipPos;
    [HideInInspector]public bool IsInteractable = false;
    [HideInInspector]public Outline TubeOutLine;
    [HideInInspector]public bool CountdownCheck = false;
    public Transform ConnectorPos;
    private Connector connector;
    public LayerMask ConnectorLayer;

    public LayerMask BarrelLayer;
    private Outline BarrelOutLine;

    public LayerMask SinkLayer;
    private Outline Sinkoutline;

    private float targetarmPos = 2.143f;
    private float zPos = 2.143f;

    public Outline LampOutLine;


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
                    }
                }
            }
            connector = null;
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
            targetarmPos = 2.143f;
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
                            sink.transform.GetChild(0).gameObject.SetActive(true);
                            StartCoroutine(DisableGameObjectAfterDelay(sink.transform.GetChild(0).gameObject, .566f));
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
        
        if(Object == null || (Object.gameObject.layer == LayerMask.NameToLayer("Default") && !IsInteractable))
        {
            targetXRot = Input.GetMouseButton(1) ? 85f : 0f;
            xRot = Mathf.Lerp(xRot, targetXRot, Time.deltaTime * rotationSpeed);

            armTarget.localRotation = Quaternion.Euler(0f, 0f, xRot);
        }

        else if (Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit raycastHit, 2f, LayerMask.NameToLayer("NailLayer")))
        {
            if (raycastHit.transform.childCount > 0 && raycastHit.transform.GetChild(0).childCount > 0 && !(raycastHit.transform.GetChild(0).GetChild(0).childCount > 0))
            {
                if(Object.gameObject.layer == LayerMask.NameToLayer("Nail") || Object.gameObject.layer == LayerMask.NameToLayer("BronzePlate"))
                {
                    Object.CountdownCheck = false;
                    Object.targetPoint = 0f;
                    Object.targetTime = 10f;

                    if (raycastHit.transform.TryGetComponent(out TubeOutLine))
                    {
                        TubeOutLine.enabled = true;
                        if (Input.GetKeyDown(KeyCode.Mouse1))
                        {
                            Object.transform.GetChild(2).gameObject.SetActive(true);
                            StartCoroutine(DisableGameObjectAfterDelay(Object.transform.GetChild(2).gameObject, .5f));
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
                        }
                    }
                }
            }
        }

        else if (Object.gameObject.layer == LayerMask.NameToLayer("Note"))
        {
            targetXRot = 0f;
            xRot = Mathf.Lerp(xRot, targetXRot, Time.deltaTime * rotationSpeed);

            armTarget.localRotation = Quaternion.Euler(0f, 0f, xRot);
            if (Input.GetMouseButtonDown(1))
            {
                targetarmPos = -4f;
            }
            else if(Input.GetMouseButtonUp(1))
            {
                targetarmPos = 2.143f;
            }
        }

        if (Object != null && (((Object.gameObject.layer != LayerMask.NameToLayer("Nail")) && Object.gameObject.layer != LayerMask.NameToLayer("BronzePlate")) || !Physics.Raycast(PlayerCam.position, PlayerCam.forward, out _, 2f, LayerMask.NameToLayer("NailLayer"))) && TubeOutLine != null)
        {
            TubeOutLine.enabled = false;
        }
        else if (Object == null && TubeOutLine != null)
        {
            TubeOutLine.enabled = false;
        }
        if (targetarmPos == -4f)
        {
            zPos = Mathf.Lerp(zPos, targetarmPos, Time.deltaTime * 5f);
        }
        else if(targetarmPos == 2.143f)
        {
            zPos = Mathf.Lerp(zPos, targetarmPos, Time.deltaTime * 2f);
        }

        armTarget.localPosition = new Vector3(0f, 0f, zPos);
        if (Object != null && connector != null && (connector.ConnectedWire == Object.transform))
        {
            connector.ConnectedWire = null;
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
                    LongNailOutLine.enabled = true;
                    if(Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        IsInteractable = true;
                        Object.IsClipOnNail = true;
                        if (Object.transform.parent.name == "BlackCable1")
                        {
                            Object.targetClip = TargetClipPos[0];
                        }
                        else if (Object.transform.parent.name == "BlackCable2")
                        {
                            Object.targetClip = TargetClipPos[1];
                        }
                        else
                        {
                            Object.targetClip = null;
                        }
                    }
                    else
                    {
                        IsInteractable = false;
                    }
                }
                else
                {
                    LongNailOutLine.enabled = false;
                }
            }

            else if (Object.CompareTag("RedClip"))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 2f, OutLineLayer))
                {
                    ShortNailOutLine.enabled = true;
                    if(Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        IsInteractable = true;
                        Object.IsClipOnNail = true;
                        if (Object.transform.parent.name == "RedCable1")
                        {
                            Object.targetClip = TargetClipPos[2];
                        }
                        else if (Object.transform.parent.name == "RedCable2")
                        {
                            Object.targetClip = TargetClipPos[3];
                        }
                        else
                        {
                            Object.targetClip = null;
                        }
                    }
                    else
                    {
                        IsInteractable = false;
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
                                    BarrelOutLine.transform.GetChild(4).GetComponent<AudioSource>().Play();
                                }
                                else if (lv.liquidLayers[1].amount == 0f)
                                {
                                    lv.liquidLayers[1].color = BarrelOutLine.GetComponent<BarrelLiquidColor>().color;
                                    lv.liquidLayers[1].amount += 0.2f;
                                    lv.UpdateLayers(true);
                                    BarrelOutLine.enabled = false;
                                    BarrelOutLine.transform.GetChild(4).GetComponent<AudioSource>().Play();
                                }
                            }
                            IsInteractable = true;
                        }
                        IsInteractable = false;
                    }
                }
                else if(BarrelOutLine != null)
                {
                    BarrelOutLine.enabled = false;
                }
            }
            else if (Object.CompareTag("Match"))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 2f))
                {
                    if(raycastHit.transform.CompareTag("Lamp") && raycastHit.transform.TryGetComponent(out LampOutLine))
                    {
                        LampOutLine.enabled = true;
                        if (Input.GetKeyDown(KeyCode.Mouse1))
                        {
                            Object.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                            LampOutLine.enabled = false;
                        }
                    }
                    else if (LampOutLine != null)
                    {
                        LampOutLine.enabled = false;
                    }
                }
                
                else if (LampOutLine != null)
                {
                    LampOutLine.enabled = false;
                }
                
            }
        }
        else
        {
            LongNailOutLine.enabled = false;
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
            if (LampOutLine != null)
            {
                LampOutLine.enabled = false;
            }
        }
        
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
    IEnumerator DisableGameObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}