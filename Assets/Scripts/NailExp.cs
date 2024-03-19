using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailExp : MonoBehaviour
{
    public Material material;
    public Transform Level;
    public Transform NailLevel;
    public GameManager gameManager;

    void Start() 
    {
        material.SetFloat("_BlendThreshold", 0);
    }
    void Update()
    {
        if(gameManager.CountdownCheck)
        {
            NailLevel.localPosition = new Vector3(NailLevel.localPosition.x, Level.localPosition.y, NailLevel.localPosition.z);
        }
        
        ChangeMatBlendThreshold();
    }
    void ChangeMatBlendThreshold()
    {
        if (material != null)
        {
            // Set the blend threshold property in the material
            material.SetFloat("_BlendThreshold", NailLevel.position.y);
        }
        else
        {
            Debug.LogError("Material not assigned.");
        }
    }
}
