    using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Text;
using LiquidVolumeFX;

public class UiManager : MonoBehaviour
{
    public LayerMask layerMask;
    public TextMeshProUGUI Objectname;
    private LiquidVolume lv;
    public Color[] color;
    public Color color1;
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

            Objectname.text = raycastHit.transform.name;

            if (Objectname.text == "End" || Objectname.text == "Start" || raycastHit.transform.gameObject.layer == LayerMask.NameToLayer("Wire"))
            {
                Objectname.text = raycastHit.transform.parent.name;
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
                            Objectname.text += " (H2O)";
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

                        if (lv.liquidLayers[2].amount > 0.01f && ColorCheck(lv.liquidLayers[2].color, color1))
                        {
                            Objectname.text += " (Fe)";
                            if (raycastHit.transform.name == "MetalContainer")
                            {
                                Objectname.text += " " + (int)(lv.liquidLayers[2].amount * 100) + "%";
                            }
                        }
                    }
                }
            }

        }
        else
        {
            Objectname.text = null;
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
                modifiedStr += " "; // Add a space before uppercase character
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

        bool check = (r - r1 <= 1) && (g - g1 <= 1) && (b - b1 <= 1);

        return check;
    }
}
