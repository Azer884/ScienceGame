using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailExp : MonoBehaviour
{
    public Material material;
    public Transform Level;
    public Vector3 NailLevel;

    void Start() 
    {
        material.SetFloat("_BlendThreshold", -10);
    }
    void Update()
    { 
        NailLevel = transform.position - Level.position;
        
        ChangeMatBlendThreshold();
    }
    void ChangeMatBlendThreshold()
    {
        if (material != null)
        {
            material.SetFloat("_BlendThreshold", NailLevel.y);
        }
    }
}
