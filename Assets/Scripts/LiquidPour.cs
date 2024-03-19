using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;
using UnityEditor;

public class LiquidPour : MonoBehaviour
{
        private Quaternion OriginalRotation;

    	private float fillSpeed = 0.01f;
        private LiquidVolume lv;
        
        private float yMax;
        private float yMin;
    // Start is called before the first frame update
    void Start()
    {
        lv = transform.parent.GetComponent<LiquidVolume> ();
        OriginalRotation = transform.localRotation;
        yMax = transform.parent.Find("TopLevel").localPosition.y;
        yMin = transform.parent.Find("BottomLevel").localPosition.y;
    }
    private void Update() {
        transform.SetLocalPositionAndRotation(Vector3.up * WaterCheckPos(lv, yMax, yMin), OriginalRotation);
    }

    void OnParticleCollision(GameObject other)
    { 
        if(other.layer == LayerMask.NameToLayer("Water")){
            if (lv.liquidLayers[0].amount < MaxWaterAmount(lv)) 
            {
                lv.liquidLayers[0].amount += fillSpeed;
                lv.UpdateLayers(true);
            }
            if (lv.level >= 1f) 
            {
                transform.localRotation = Quaternion.Euler (Random.value * 30 - 15, Random.value * 30 - 15, Random.value * 30 - 15);
            } 
            else 
            {
                transform.localRotation = Quaternion.Euler (0, 0, 0);
            }

            lv.UpdateLayers(true);
        }
        else if(other.layer == LayerMask.NameToLayer("Powder"))
        {
            if (lv.liquidLayers[2].amount < .25f)
            {
                lv.liquidLayers[2].amount += fillSpeed * .1f;

                lv.UpdateLayers(true);
            }
        }
        else if(other.layer == LayerMask.NameToLayer("Copper"))
        {
            if (lv.liquidLayers[1].amount < .25f)
            {
                lv.liquidLayers[1].amount += fillSpeed * .2f;

                lv.UpdateLayers(true);
            }
        }
    }
        

    float MaxWaterAmount(LiquidVolume lv)
    {
        float maxWaterAmount = 1f;
        for (int i = 1; i < lv.liquidLayers.Length; i++)
        {
            maxWaterAmount -= lv.liquidLayers[i].amount;
        }

        return maxWaterAmount;
    }

    float WaterCheckPos(LiquidVolume lv, float yMax, float yMin)
    {
        float y;
        y = yMin + (yMax - yMin) * lv.level;

        return y;
    }
}