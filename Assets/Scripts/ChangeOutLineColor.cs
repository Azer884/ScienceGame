using System.Collections;
using System.Collections.Generic;
using LiquidVolumeFX;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOutLineColor : MonoBehaviour
{
    private Outline outline;
    // Start is called before the first frame update
    void Start()
    {
        outline = transform.GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        if (outline.transform.childCount > 1 && outline.transform.GetChild(1).TryGetComponent(out LiquidVolume lv))
        {
            if (lv.liquidLayers[1].amount > .1f && UiManager.ColorCheck(lv.liquidLayers[1].color, FindAnyObjectByType<UISpawner>().color[1]))
            {
                outline.OutlineColor = Color.green;
            }
            else
            {
                outline.OutlineColor = Color.white;
            }
        }
    }
}
