using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    [HideInInspector]public bool IsOpened = false;
    public GameObject Water;
    
    void Update()
    {
        if (IsOpened)
        {
            transform.SetLocalPositionAndRotation(Vector3.Lerp(transform.localPosition, new Vector3(0.000351f, -0.00044f, -0.001586f), Time.deltaTime * 5f), Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-76.758f, -86.16f, 72.2f), Time.deltaTime * 5f));
            Water.SetActive(false);
        }
        else
        {
            transform.SetLocalPositionAndRotation(Vector3.Lerp(transform.localPosition, new Vector3(0.000351f, -0.00044f, -0.001586f), Time.deltaTime * 5f), Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-9.667f, 2.344f, -9.439f), Time.deltaTime * 5f));
            Water.SetActive(true);
        }
    }
    
}
