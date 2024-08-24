using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField]private LiquidPropertiesCollection liquids;
    private void OnTriggerEnter(Collider other) {
            Movement movement = other.transform.GetComponentInParent<Movement>();
            movement.isSwimming = true;
        
    }
    private void OnTriggerExit(Collider other) {
            Movement movement = other.transform.GetComponentInParent<Movement>();
            movement.isSwimming = false;
        
    }
    
}
