using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class PouringSystem : MonoBehaviour
{
    private LiquidVolume liquid;
    private ParticleSystem particleSystem;


    // Start is called before the first frame update
    void Start()
    {
        liquid = gameObject.GetComponent<LiquidVolume>();
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Angle(Vector3.down, transform.parent.forward) <= 55f || Vector3.Angle(Vector3.down, transform.parent.right) <= 55f || Vector3.Angle(Vector3.down, -transform.parent.forward) <= 55f || Vector3.Angle(Vector3.down, -transform.parent.right) <= 55f)
        {
            for (int i = 0; i < liquid.liquidLayers.Length; i++)
            {
                if(liquid.liquidLayers[i].amount > 0.15f)
                {
                    liquid.liquidLayers[i].amount -= Time.deltaTime * liquid.liquidLayers[i].amount * 0.75f;
                }
                else
                {
                    liquid.liquidLayers[i].amount -= Time.deltaTime * (liquid.liquidLayers[i].amount + .15f) * 0.75f;
                }
                liquid.UpdateLayers(true);
            }

            if (LiquidCheck(liquid))
            {
                particleSystem.Stop();
            }
            else
            {
                particleSystem.Play();
            }
        }
        else
        {
            particleSystem.Stop();
        }

        if (LiquidCheck(liquid))
        {
            liquid.alpha = 0f;
        }
        else
        {
            liquid.alpha = 1f;
        }
    }

    bool LiquidCheck(LiquidVolume lv)
    {
        bool level = true;
        for (int i = 0; i < lv.liquidLayers.Length; i++)
        {
            if (lv.liquidLayers[i].amount <= 0.02f)
            {
                level = true;
            }
            else
            {
                level = false;
                break;
            }
        }
        return level;
    }
}
