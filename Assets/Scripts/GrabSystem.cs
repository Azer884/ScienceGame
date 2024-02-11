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

    private float rotationSpeed = 5f;
    private float targetXRot = 0f; // Target value for xRot

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

        targetXRot = Input.GetMouseButton(1) ? 85f : 0f; // Set target value based on mouse button state
        xRot = Mathf.Lerp(xRot, targetXRot, Time.deltaTime * rotationSpeed); // Smoothly interpolate xRot
        
        armTarget.localRotation = Quaternion.Euler(0f, 0f, xRot);

        if (Object != null)
        {
            Object.transform.rotation = armTarget.rotation;
        }
    }
}
