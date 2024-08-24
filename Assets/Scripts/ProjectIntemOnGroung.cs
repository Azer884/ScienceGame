using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectIntemOnGroung : MonoBehaviour
{
    private void Awake() {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit))
        {
            transform.position = raycastHit.point;
        }
    }
}
