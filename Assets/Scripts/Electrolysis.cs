using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class Electrolysis : MonoBehaviour
{
    private LiquidVolume lv;
    public GameObject bubbles;

    
    void Start()
    {
        lv = transform.parent.parent.GetComponentInChildren<LiquidVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lv.liquidLayers[0].amount >= 0.6f)
        {
            /*if (electricity)
            {*/
                bubbles.SetActive(true);
            //}
            /*else
            {
                bubbleParticles.Stop();
            }*/
        }
        else
        {
            bubbles.SetActive(false);
        }
    }
}
