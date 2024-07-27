using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoTrigger : MonoBehaviour
{
    public bool Entered;
    private void OnTriggerEnter(Collider other)
    {
        Entered = true;
    }   
}
