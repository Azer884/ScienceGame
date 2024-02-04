using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class LiquidPour : MonoBehaviour
{

    	private float fillSpeed = 0.01f;
        private LiquidVolume lv;
    // Start is called before the first frame update
    void Start()
    {
        lv = transform.parent.GetComponent<LiquidVolume> ();
    }
    private void Update() {
        transform.position = new Vector3(0f, lv.liquidSurfaceYPosition - transform.localScale.y * 0.5f, 0f);
    }

    void OnParticleCollision(GameObject other)
    {
        if (lv.liquidLayers[0].amount < MaxWaterAmount(lv)) {
            lv.liquidLayers[0].amount += fillSpeed;
            lv.UpdateLayers(true);
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
}
