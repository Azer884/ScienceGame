using TMPro;
using UnityEngine;
using System.Text;
using System;

public class UiManager : MonoBehaviour
{
    public LayerMask layerMask;
    public TextMeshProUGUI Objectname;
    private Liquid lv;
    public Color[] color;
    // Start is called before the first frame update
    void Start()
    {
        Objectname.text = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 10f, layerMask))
        {

            Objectname.text = raycastHit.transform.name + "\n";

            Objectname.text = RemoveNumbers(Objectname.text);
            Objectname.text = AddSpaceBeforeUppercase(Objectname.text);
            if (raycastHit.transform.childCount > 1)
            {
                if (raycastHit.transform.GetChild(1).name == "LiquidVolume")
                {
                    lv = raycastHit.transform.GetChild(1).GetComponent<Liquid>();
                    if (lv.fillAmount < 0.01f)
                    {
                        Objectname.text += " (Empty)";
                    }

                    else if(lv.fillAmount > 0.01f)
                    {

                        Objectname.text += " " + (int)(lv.fillAmount * 100) + "%";
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
        str = str.Replace("(Clone)", string.Empty);

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
