using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    [SerializeField] private Transform PlayerCam;
    [SerializeField] private Transform GrabPiont;
    [SerializeField] private LayerMask PickupLayer;
    private ObjectGrabbable Object;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
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
            else
            {
                {
                    Object.Drop();
                    Object = null;
                }
            }
        }
    }
}
