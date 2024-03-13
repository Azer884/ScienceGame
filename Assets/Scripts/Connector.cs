using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public bool Connected = false;
    public Transform ConnectedWire;

    void Update()
    {
        if (ConnectedWire != null)
        {
            Connected = true;
        }
        else
        {
            Connected = false;
        }
    }
}
