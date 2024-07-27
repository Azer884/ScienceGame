using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public bool Connected = false;
    [HideInInspector]public Transform ConnectedWire;

    void Update()
    {
        if (ConnectedWire != null)
        {
            Connected = true;
            transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            Connected = false;
            transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
