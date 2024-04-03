    using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text;
using LiquidVolumeFX;
using System;

public class UiManager : MonoBehaviour
{
    public LayerMask layerMask;
    public TextMeshProUGUI Objectname;
    private LiquidVolume lv;
    public Color[] color;
    public Color[] PowerColor;
    // Start is called before the first frame update
    void Start()
    {
        Objectname.text = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 2f, layerMask))
        {

            Objectname.text = raycastHit.transform.name + "\n";

            if (raycastHit.transform.name == "End" || raycastHit.transform.name == "Start" || raycastHit.transform.gameObject.layer == LayerMask.NameToLayer("Wire"))
            {
                Objectname.text = raycastHit.transform.parent.name + "\n";
            }

            Objectname.text = RemoveNumbers(Objectname.text);
            Objectname.text = AddSpaceBeforeUppercase(Objectname.text);
            if (raycastHit.transform.childCount > 1)
            {
                if (raycastHit.transform.GetChild(1).name == "LiquidVolume" || raycastHit.transform.GetChild(1).name == "PowderVolume")
                {
                    lv = raycastHit.transform.GetChild(1).GetComponent<LiquidVolume>();
                    if (lv.level < 0.01f)
                    {
                        Objectname.text += " (Empty)";
                    }

                    else {
                        if (lv.liquidLayers[0].amount > 0.01f)
                        {
                            Objectname.text += " (H<sub>2</sub>O)";
                        }

                        if (lv.liquidLayers[1].amount > 0.01f && ColorCheck(lv.liquidLayers[1].color, color[0]))
                        {
                            Objectname.text += " (Cu<sup>2+</sup> , SO<sub>4</sub><sup>2-</sup>)";
                        }
                        else if (lv.liquidLayers[1].amount > 0.01f && ColorCheck(lv.liquidLayers[1].color, color[1]))
                        {
                            Objectname.text += " (Fe<sup>2+</sup>)";
                        }
                        else if (lv.liquidLayers[1].amount > 0.01f && ColorCheck(lv.liquidLayers[1].color, color[2]))
                        {
                            Objectname.text += " (H<sub>3</sub>O<sup>+</sup> , Cl<sup>-</sup>)";
                        }
                        else if (lv.liquidLayers[1].amount > 0.01f && ColorCheck(lv.liquidLayers[1].color, color[3]))
                        {
                            Objectname.text += " (Ag<sup>+</sup> , NO<sub>3</sub><sup>-</sup>)";
                        }
                        else if (lv.liquidLayers[1].amount > 0.01f && ColorCheck(lv.liquidLayers[1].color, color[4]))
                        {
                            Objectname.text += " (Cu<sup>2+</sup>)";
                        }
                        else if (lv.liquidLayers[1].amount > 0.01f && ColorCheck(lv.liquidLayers[1].color, color[5]))
                        {
                            Objectname.text += " (Na<sup>+</sup>, OH<sup>-</sup>)";
                        }

                        if (lv.liquidLayers[2].amount > 0.01f && ColorCheck(lv.liquidLayers[2].color, PowerColor[0]))
                        {
                            Objectname.text += " (Fe)";
                            if (raycastHit.transform.name == "MetalContainer")
                            {
                                Objectname.text += " " + (int)(lv.liquidLayers[2].amount * 100) + "%";
                            }
                        }
                        if (lv.liquidLayers.Length > 3 && lv.liquidLayers[3].amount > 0.001f && ColorCheck(lv.liquidLayers[3].color, PowerColor[1]))
                        {
                            Objectname.text += " (NaCl)";
                            if (raycastHit.transform.name == "SaltContainer")
                            {
                                Objectname.text += " " + (int)(lv.liquidLayers[3].amount * 100) + "%";
                            }
                        }
                    }
                }
            }

        }
        else
        {
            Invoke(nameof(EmptyName), 1f);
        }
    }

    string AddSpaceBeforeUppercase(string str)
    {
        string modifiedStr = str[0].ToString();

        for (int i = 1; i < str.Length; i++)
        {
            char c = str[i];
            if (char.IsUpper(c))
            {
                modifiedStr += " ";
            }
            modifiedStr += c;
        }

        return modifiedStr;
    }

    string RemoveNumbers(string str)
    {
        StringBuilder result = new();

        foreach (char c in str)
        {
            if (!char.IsDigit(c))
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }

    public static bool ColorCheck(Color lv, Color SecondColor)
    {
        int r = (int)(lv.r * 1000);
        int g = (int)(lv.g * 1000);
        int b = (int)(lv.b * 1000);

        int r1 = (int)(SecondColor.r * 1000);
        int g1 = (int)(SecondColor.g * 1000);
        int b1 = (int)(SecondColor.b * 1000);

        bool check = (Math.Abs(r - r1) <= 1) && (Math.Abs(g - g1) <= 1) && (Math.Abs(b - b1) <= 1);

        return check;
    }
    void EmptyName()
    {
        Objectname.text = null;
    }
}
