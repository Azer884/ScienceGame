using UnityEngine;

[System.Serializable]
public struct LiquidProperties
{
    public string name;
    [ColorUsage(true, true)]
    public Color liquidColor;
    [ColorUsage(true, true)]
    public Color topColor;
    public float maxWobble;
    public float thickness;
    [Range(0f, 0.5f)]
    public float foamSmoothness;
    public Color foamColor;
    public float foamWidth;
    public float rimPower;
    public Color rimColor;
}
