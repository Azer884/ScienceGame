using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityCheck : MonoBehaviour
{
    public bool ElectricityOn = false;
    private bool start = false;
    private bool end = false;
    
    void Update()
    {
        if (transform.GetChild(0).TryGetComponent(out ObjectGrabbable objectGrabbable))
        {
            if (objectGrabbable.Electricity)
            {
                start = true;
            }
        }
        if (transform.GetChild(1).TryGetComponent(out ObjectGrabbable grabbable))
        {
            if (grabbable.targetClip != null)
            {
                end = true;
            }
        }
        if (start && end)
        {
            ElectricityOn = true;
        }
        else
        {
            ElectricityOn = false;
        }
    }
}
