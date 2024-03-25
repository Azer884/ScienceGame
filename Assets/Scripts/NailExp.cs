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

        // Calculate local Y position relative to the parent
        float localY = transform.localPosition.y;

        // Set the blend threshold based on local Y position
        material.SetFloat("_BlendThreshold", localY);
    }
}
