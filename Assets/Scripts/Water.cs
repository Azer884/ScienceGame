using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField]private LiquidPropertiesCollection liquids;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("EyeLevel"))
        {
            Movement movement = other.transform.GetComponentInParent<Movement>();
            movement.isSwimming = true;
            movement.ResetVelocity();
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("EyeLevel"))
        {
            Movement movement = other.transform.GetComponentInParent<Movement>();
            movement.isSwimming = false;
            movement.ResetVelocity();
        }
    }
    
}
