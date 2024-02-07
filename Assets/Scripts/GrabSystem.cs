using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    [SerializeField] private Transform PlayerCam;
    [SerializeField] private Transform GrabPiont;
    [SerializeField] private LayerMask PickupLayer;
    private ObjectGrabbable Object;
    private float xRot;
    public Transform armTarget;

    private float Multi = 100f;

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
            {
                Object.Drop();
                Object = null;
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            xRot = 0f;
        }
        if (Input.GetMouseButton(1))
        {
            xRot = Mathf.Clamp(xRot, 0f, 85f);
            xRot += Time.deltaTime * Multi;
            armTarget.localRotation = Quaternion.Euler(0f, 0f, xRot);
            Debug.Log(xRot);

            Object.transform.rotation = armTarget.rotation;
        }
        else
        {
            xRot = Mathf.Clamp(xRot, 0f, 85f);
            xRot -= Time.deltaTime * Multi;
            armTarget.localRotation = Quaternion.Euler(0f, 0f, xRot);
            Object.transform.rotation = armTarget.rotation;
        }
    }
}
