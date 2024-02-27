using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class FireCheck : MonoBehaviour
{
    private LiquidVolume lv;
    private new ParticleSystem particleSystem;
    private float targetTime = 3f;
    private Coroutine countdownCoroutine;

    void Start()
    {
        particleSystem = transform.parent.parent.Find("Explotion FX").GetComponent<ParticleSystem>();
        particleSystem.gameObject.SetActive(false);

        lv = transform.parent.GetComponent<LiquidVolume> ();

        countdownCoroutine = null; // Initialize the countdownCoroutine variable to null
    }

    private void OnParticleCollision(GameObject other)
    {
        if(lv.liquidLayers[2].amount > .1f)
        {
            if (countdownCoroutine == null)
            {
                countdownCoroutine = StartCoroutine(Countdown());
            }
        }
        else
        {
            if(countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
                targetTime = 3f;
                particleSystem.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator Countdown()
    {
        targetTime = 3f;
        while (targetTime > 0)
        {
            yield return new WaitForSeconds(1);
            targetTime--;
        }

        lv.liquidLayers[2].amount /= lv.liquidLayers[2].density; 
        lv.liquidLayers[2].density = 1f ;
        lv.liquidLayers[2].miscible = true ;

        particleSystem.gameObject.SetActive(true);
        lv.UpdateLayers(true);

        countdownCoroutine = null;
    }
}