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
    private bool Check = false;
    private float TargetTime;

    void Start()
    {
        particleSystem = transform.parent.parent.Find("Explotion FX").GetComponent<ParticleSystem>();
        particleSystem.gameObject.SetActive(false);

        lv = transform.parent.GetComponent<LiquidVolume> ();

        countdownCoroutine = null;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(lv.liquidLayers[2].amount > .1f && lv.liquidLayers[1].amount > 0.1f && UiManager.ColorCheck(lv.liquidLayers[1].color, FindAnyObjectByType<UiManager>().color[2]))
        {
            countdownCoroutine ??= StartCoroutine(Countdown());
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

        Check = true;
        countdownCoroutine = null;
    }
    void Update() 
    {
        if (Check)
        {
            lv.liquidLayers[1].color = Color.Lerp(lv.liquidLayers[1].color ,FindAnyObjectByType<UiManager>().color[1], TargetTime);
            lv.UpdateLayers(true);
            TargetTime += Time.deltaTime * .0001f;
            lv.liquidLayers[2].amount = 0f;
            particleSystem.gameObject.SetActive(true);
            if (lv.liquidLayers[1].color == FindAnyObjectByType<UiManager>().color[1])
            {
                Check = false;
                TargetTime = 0f;
            }
        }
    }
}