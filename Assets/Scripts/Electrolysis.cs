using UnityEngine;
using LiquidVolumeFX;
using System.Collections;

public class Electrolysis : MonoBehaviour
{
    private LiquidVolume lv;
    public GameObject bubbles;
    public Transform[] Clips;
    public ElectricityCheck[] Cables;
    private float targetTime = 43f;
    private Coroutine countdownCoroutine;
    private bool Check = false;
    private Color WaterColor;
    public bool OutlineCheck = false;

    
    void Start()
    {
        lv = transform.parent.parent.GetComponentInChildren<LiquidVolume>();
        lv.liquidLayers[1].color = lv.liquidLayers[0].color;
        WaterColor = lv.liquidLayers[0].color;
    }

    // Update is called once per frame
    void Update()
    {
        if(countdownCoroutine != null && targetTime <= 0)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
            targetTime = 43f;
        }
        BubbleSys();
        if (Check)
        {
            lv.liquidLayers[1].color = FindAnyObjectByType<UiManager>().color[5];
            lv.liquidLayers[0].color = Color.Lerp(lv.liquidLayers[0].color, FindAnyObjectByType<UiManager>().color[5], Time.deltaTime * .001f);
            lv.liquidLayers[3].amount = Mathf.Lerp(lv.liquidLayers[3].amount, 0f, Time.deltaTime * .001f);
            lv.UpdateLayers(true);
        }
    }



    void BubbleSys(){
        if (lv.liquidLayers[0].amount >= 0.5f && lv.liquidLayers[3].amount > 0f)
        {
            if (CheckTransforms(Clips, Cables))
            {
                if (CheckElectricity(Cables))
                {
                    bubbles.SetActive(true);
                    countdownCoroutine ??= StartCoroutine(Countdown());
                }
                else
                {
                    bubbles.SetActive(false);
                }
            }
            else
            {
                bubbles.SetActive(false);
            }
        }
        else
        {
            bubbles.SetActive(false);
        }
    }
    private bool CheckTransforms(Transform[] transforms, ElectricityCheck[] transforms1)
    {
        bool v = true;
        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms1[i].TryGetComponent(out ObjectGrabbable grabbable))
            {
                if (grabbable.targetClip != transforms[i])
                {
                    v = false;
                    break;
                }
            }
        }

        return v;
    }

    private bool CheckElectricity(ElectricityCheck[] electricity)
    {
        bool v = true;
        for (int i = 0; i < electricity.Length ; i++)
        {
            if (!electricity[i].ElectricityOn)
            {
                v = false;
                break;
            }
        }

        return v;
    }

    private IEnumerator Countdown()
    {
        targetTime = 43f;
        while (targetTime > 0)
        {
            yield return new WaitForSeconds(1);
            targetTime--;
            Check = true;
        }

        lv.liquidLayers[3].amount = 0f;
        lv.liquidLayers[1].amount = lv.liquidLayers[0].amount;
        lv.liquidLayers[0].amount = 0f;
        lv.UpdateLayers(true);
        Check = false;
        lv.liquidLayers[0].color = WaterColor;
        OutlineCheck = true;
        
        countdownCoroutine = null;
    }
}
