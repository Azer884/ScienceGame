using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform PlayerCam;
    [SerializeField] private LayerMask PickupLayer;
    [HideInInspector]public ObjectGrabbable Object;
    private float xRot;
    public Transform armTarget;

    private readonly float rotationSpeed = 5f;
    private float targetXRot = 0f;
    [HideInInspector]public bool IsInteractable = false;
    public LayerMask SinkLayer;
    [SerializeField]private float pickUpDistance = 7f;
    [SerializeField]private LiquidPropertiesCollection liquids;


    private void Update()
    {
        //Grab
        if (Input.GetMouseButtonDown(0))
        {
            if(Object == null)
            {
                if (Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit raycastHit, pickUpDistance, PickupLayer))
                {
                    if (raycastHit.transform.TryGetComponent(out Object) && !IsInteractable)
                    {
                        Object.Grab(transform);
                    }
                }
            }
        }
        //Drop
        else if (Input.GetMouseButtonUp(0) && !IsInteractable)
        {
            if(Object != null)
            {
                Object.Drop();
            }
            Object = null;
        }

        //SetObjOnPosition();
        
        if (IsInteractable)
        {
            Object = null;
        }

        if (Object != null && !IsInteractable)
        {
            Object.transform.rotation = armTarget.rotation;
        }

        //Opennig/Closing the sinnk
        if(Object == null && Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit sink, pickUpDistance, SinkLayer))
        {
            if(sink.transform.name == "SinkTap")
            {

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
                    }
                
            }
        }
        
        //Rotate arm
        if(Object == null || (Object.gameObject.layer == LayerMask.NameToLayer("Default"/*change to sword*/) && !IsInteractable))
        {
            targetXRot = Input.GetMouseButton(1) ? 85f : 0f;
            xRot = Mathf.Lerp(xRot, targetXRot, Time.deltaTime * rotationSpeed);

            armTarget.localRotation = Quaternion.Euler(0f, 0f, xRot);
        }


        UseItem();
    }

    IEnumerator DisableGameObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    private void UseItem()
    {
        if (Object && Object.gameObject.layer == LayerMask.NameToLayer("Grabbable") && Physics.Raycast(PlayerCam.position, PlayerCam.forward, out RaycastHit raycastHit, pickUpDistance))
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (raycastHit.transform.parent.GetComponentInChildren<Liquid>())
                {
                    Liquid container = raycastHit.transform.parent.GetComponentInChildren<Liquid>();
                    
                    if (!container.combos.stringLists.Contains(Object.gameObject.tag))
                    {
                        container.combos.stringLists.Add(Object.gameObject.tag);
                    }
                    
                    container.fillAmount += .1f;
                    container.StartCoroutine(container.UpdateLiquid(container, liquids.liquidPropertiesList[TagToNum(Object.gameObject)], 1));
                    Destroy(Object.gameObject);
                    Object = null;
                }
            }
        }
    }

    private int TagToNum(GameObject Object)
    {
        return Object.tag switch
        {
            "Berry" => 1,
            "Flower" => 2,
            "Root" => 3,
            "Wood" => 4,
            "Obsidian" => 5,
            _ => 0,
        };
    }
}