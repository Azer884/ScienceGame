using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class ParticleColorChanger : MonoBehaviour
{
    // Start is called before the first frame update
    private ParticleSystem particleSystem;
    private LiquidVolume liquid;
    private Color color;

    void Start()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
        liquid = gameObject.GetComponentInParent<LiquidVolume>();
    }

    [System.Obsolete]
    void LateUpdate() {
        particleSystem.startColor = AvgColor(liquid);
    }
    // Update is called once per frame
    Color AvgColor(LiquidVolume liquid)
    {
        float r = 0f;
        float g = 0f;
        float b = 0f;
        Color color;

        for (int i = 0; i < liquid.liquidLayers.Length; i++)
        {
            r += liquid.liquidLayers[i].color.r * liquid.liquidLayers[i].amount;
            g += liquid.liquidLayers[i].color.g * liquid.liquidLayers[i].amount;
            b += liquid.liquidLayers[i].color.b * liquid.liquidLayers[i].amount;
        }
        color = new Color(r / liquid.level , g / liquid.level ,b / liquid.level);
        return color;
    }
}
