using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;
using UnityEditor;

public class LiquidPour : MonoBehaviour
{
        private Quaternion OriginalRotation;

        private float targetTime = 5f;
    	private float fillSpeed = 0.01f;
        private LiquidVolume lv;

        private new ParticleSystem particleSystem;
        
        private float yMax;
        private float yMin;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem = transform.parent.parent.Find("Explotion FX").GetComponent<ParticleSystem>();
        particleSystem.gameObject.SetActive(false);

        OriginalRotation = transform.localRotation;
        lv = transform.parent.GetComponent<LiquidVolume> ();
        yMax = transform.parent.Find("TopLevel").localPosition.y;
        yMin = transform.parent.Find("BottomLevel").localPosition.y;
    }
    private void Update() {
        transform.SetLocalPositionAndRotation(Vector3.up * WaterCheckPos(lv, yMax, yMin), OriginalRotation);

        if(lv.liquidLayers[2].amount > .1f)
        {
            targetTime -= Time.deltaTime;

            if (targetTime <= 0f)
            {
                lv.liquidLayers[2].amount /= lv.liquidLayers[2].density; 
                lv.liquidLayers[2].density = 1f ;
                lv.liquidLayers[2].miscible = true ;

                particleSystem.gameObject.SetActive(true);
            }
            else
            {
                particleSystem.gameObject.SetActive(false);
            }
            lv.UpdateLayers(true);
        }
        else
        {
            targetTime = 5f;
        }
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
        y = yMax * (2 * lv.level -1) ;

        if(y <= yMin)
        {
            y = yMin;
        }
        return y;
    }
}