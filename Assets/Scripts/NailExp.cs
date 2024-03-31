using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailExp : MonoBehaviour
{
    public Material material;
    public Transform Level;
    private ObjectGrabbable objectGrabbable;
    private bool needsTextureBake = false; // Local variable for texture baking flag

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

        // Check if a new texture needs to be baked
        if (needsTextureBake)
        {
            StartCoroutine(BakeMixedTexture());
            needsTextureBake = false; // Reset the flag
        }
        if (Input.GetKey(KeyCode.Space))
        {
            SetNeedsTextureBake(true);
        }
    }

    public void SetNeedsTextureBake(bool value)
    {
        needsTextureBake = value;
    }

    IEnumerator BakeMixedTexture()
    {
        // Create a render texture for the mixed texture
        RenderTexture mixedTexture = new RenderTexture(512, 512, 0);
        Camera camera = Camera.main; // Assuming you have a main camera

        // Set the camera's target texture to mixedTexture
        camera.targetTexture = mixedTexture;

        // Clear the render texture and render the object with the mixed material
        camera.clearFlags = CameraClearFlags.SolidColor; // Clear to background color
        camera.backgroundColor = Color.clear; // Set background color to transparent
        camera.Render();

        // Create a temporary texture to hold the mixed result
        Texture2D tempTexture = new Texture2D(512, 512);
        
        // Read pixels from mixedTexture into tempTexture
        RenderTexture.active = mixedTexture;
        tempTexture.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
        tempTexture.Apply();
        
        // Optional: Save the mixed texture to a file
        byte[] bytes = tempTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes("Assets/Textures/MixedTexture.png", bytes);

        // Release the render texture and camera target texture
        RenderTexture.active = null;
        camera.targetTexture = null;
        Destroy(mixedTexture);

        // Set the mixed texture to the material for visualization
        material.mainTexture = tempTexture;

        yield return null;
    }
}
