using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailExp : MonoBehaviour
{
    public Material material;
    public Transform Level;
    private ObjectGrabbable objectGrabbable;

    void Start() 
    {
        objectGrabbable = transform.parent.GetComponent<ObjectGrabbable>();
    }
    void Update()
    {
        if (objectGrabbable.CountdownCheck)
        {
            transform.position = Level.position;
        }
        material.SetFloat("_BlendThreshold", transform.position.y);
    }
}
