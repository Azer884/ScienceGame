using UnityEngine;
using LiquidVolumeFX;

public class Electrolysis : MonoBehaviour
{
    private LiquidVolume lv;
    public GameObject bubbles;
    public Transform[] Clips;
    public ElectricityCheck[] Cables;

    
    void Start()
    {
        lv = transform.parent.parent.GetComponentInChildren<LiquidVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        BubbleSys();
    }



    void BubbleSys(){
        if (lv.liquidLayers[0].amount >= 0.5f && lv.liquidLayers[3].amount >= .1f)
        {
            if (CheckTransforms(Clips, Cables))
            {
                if (CheckElectricity(Cables))
                {
                    bubbles.SetActive(true);
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
}
