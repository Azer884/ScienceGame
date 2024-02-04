using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class LiquidPour : MonoBehaviour
{

    	private float fillSpeed = 0.01f;
        private LiquidVolume lv;
        private Vector3 thePosition;

        private float yMax;
    // Start is called before the first frame update
    void Start()
    {
        lv = transform.parent.GetComponent<LiquidVolume> ();
        yMax = transform.parent.Find("TopLevel").localPosition.y;
    }
    private void Update() {
        transform.localPosition = new Vector3(0f, WaterCheckPos(lv,yMax), 0f);
        transform.localRotation = Quaternion.Euler (0, 0, 0);
    }

    void OnParticleCollision(GameObject other)
    {
        if (lv.liquidLayers[0].amount < MaxWaterAmount(lv)) {
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

    float WaterCheckPos(LiquidVolume lv, float yMax)
    {
        float y;
        y = yMax * (2 * lv.level -1) ;
        
        return y;
    }
}
