using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public enum UpdateMode { Normal, UnscaledTime }
    public UpdateMode updateMode;
    [ColorUsage(true, true)]
    public Color color;
    [ColorUsage(true, true)]
    public Color topColor;
    public float MaxWobble = 0.03f;
    [SerializeField]
    float WobbleSpeedMove = 1f;
    public float fillAmount = 0.5f;
    [SerializeField]
    float Recovery = 1f;
    public float Thickness = 1f;
    [Range(0f, .5f)]
    public float foamSmoothness = 0;
    public Color foamColor;
    public float foamWidth;
    public float rimPower;
    public Color rimColor;

    float CompensateShapeAmount;
    [SerializeField]
    Mesh mesh;
    [SerializeField]
    Renderer rend;
    Vector3 pos;
    Vector3 lastPos;
    Vector3 velocity;
    Quaternion lastRot;
    Vector3 angularVelocity;
    float wobbleAmountX;
    float wobbleAmountZ;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;
    float pulse;
    float sinewave;
    float time = 0.5f;
    Vector3 comp;
    public PotionLists combos;
    public PotionLists potions;

    private static readonly Dictionary<string, string[]> potionCombinations = new Dictionary<string, string[]>
    {
        {"Strength Potion", new[] {"Berry", "Root", "Obsidian"}},
        {"Speed Potion", new[] {"Berry", "Root", "Wood"}},
        {"Defense Potion", new[] {"Root", "Obsidian", "Wood"}},
        {"Poison Potion", new[] {"Flower", "Obsidian", "Wood"}},
        {"Attack Speed Potion", new[] {"Flower", "Root", "Obsidian", "Wood"}},
        {"Jump Boost Potion", new[] {"Flower", "Berry", "Root", "Obsidian"}},
        {"Night Vision Potion", new[] {"Flower", "Berry", "Root"}},
        {"Scale Down Potion", new[] {"Flower", "Root", "Wood"}},
        {"Health Potion", new[] {"Flower", "Berry", "Obsidian", "Wood"}},
        {"Invisibility Potion", new[] {"Flower", "Root", "Obsidian"}},
        {"Scale Up Potion", new[] {"Berry", "Obsidian", "Wood"}},
        {"Dizziness Potion", new[] {"Flower", "Berry", "Obsidian"}},
        {"Enemy Invisibility Potion", new[] {"Flower", "Berry", "Root", "Wood"}},
        {"Scale Enemy Down Potion", new[] {"Flower", "Berry", "Wood"}},
        {"Scale Enemy Up Potion", new[] {"Berry", "Root", "Obsidian", "Wood"}},
        {"Second Life Potion", new[] {"Flower", "Berry", "Root", "Obsidian", "Wood"}}
    };

    // Use this for initialization
    void Awake()
    {
        combos.stringLists.Clear();

        GetMeshAndRend();

        color = rend.material.GetColor("_BottomColor");
        topColor = rend.material.GetColor("_TopColor");
        foamSmoothness = rend.material.GetFloat("_Foam_Smoothness");
        foamColor = rend.material.GetColor("_FoamColor");
        foamWidth = rend.material.GetFloat("_FoamWidth");
        rimPower = rend.material.GetFloat("_Rim_Power");
        rimColor = rend.material.GetColor("_Rim_Color");
    }

    private void OnValidate()
    {
        GetMeshAndRend();
    }

    void GetMeshAndRend()
    {
        if (mesh == null)
        {
            mesh = GetComponent<MeshFilter>().mesh;
        }
        if (rend == null)
        {
            rend = GetComponent<Renderer>();
        }
    }
    void Update()
    {

        float deltaTime = 0;
        switch (updateMode)
        {
            case UpdateMode.Normal:
                deltaTime = Time.deltaTime;
                break;

            case UpdateMode.UnscaledTime:
                deltaTime = Time.unscaledDeltaTime;
                break;
        }

        time += deltaTime;

        if (deltaTime != 0)
        {


            // decrease wobble over time
            wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, (deltaTime * Recovery));
            wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, (deltaTime * Recovery));



            // make a sine wave of the decreasing wobble
            pulse = 2 * Mathf.PI * WobbleSpeedMove;
            sinewave = Mathf.Lerp(sinewave, Mathf.Sin(pulse * time), deltaTime * Mathf.Clamp(velocity.magnitude + angularVelocity.magnitude, Thickness, 10));

            wobbleAmountX = wobbleAmountToAddX * sinewave;
            wobbleAmountZ = wobbleAmountToAddZ * sinewave;



            // velocity
            velocity = (lastPos - transform.position) / deltaTime;

            angularVelocity = GetAngularVelocity(lastRot, transform.rotation);

            // add clamped velocity to wobble
            wobbleAmountToAddX += Mathf.Clamp((velocity.x + (velocity.y * 0.2f) + angularVelocity.z + angularVelocity.y) * MaxWobble, -MaxWobble, MaxWobble);
            wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (velocity.y * 0.2f) + angularVelocity.x + angularVelocity.y) * MaxWobble, -MaxWobble, MaxWobble);
        }

        // send it to the shader
        rend.material.SetFloat("_WobbleX", wobbleAmountX);
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ);

        // set fill amount
        UpdatePos(deltaTime);

        // keep last position
        lastPos = transform.position;
        lastRot = transform.rotation;

        rend.material.SetColor("_BottomColor", color);
        rend.material.SetColor("_TopColor", topColor);
        rend.material.SetFloat("_Foam_Smoothness", foamSmoothness);
        rend.material.SetColor("_FoamColor", foamColor);
        rend.material.SetFloat("_FoamWidth", foamWidth);
        rend.material.SetFloat("_Rim_Power", rimPower);
        rend.material.SetColor("_Rim_Color", rimColor);

        PotionCreator();
    }

    void UpdatePos(float deltaTime)
    {

        Vector3 worldPos = transform.TransformPoint(new Vector3(mesh.bounds.center.x, mesh.bounds.center.y, mesh.bounds.center.z));
        if (CompensateShapeAmount > 0)
        {
            // only lerp if not paused/normal update
            if (deltaTime != 0)
            {
                comp = Vector3.Lerp(comp, (worldPos - new Vector3(0, GetLowestPoint(), 0)), deltaTime * 10);
            }
            else
            {
                comp = (worldPos - new Vector3(0, GetLowestPoint(), 0));
            }

            pos = worldPos - transform.position - new Vector3(0, -fillAmount - (comp.y * CompensateShapeAmount), 0);
        }
        else
        {
            pos = worldPos - transform.position - new Vector3(0, -fillAmount, 0);
        }
        rend.material.SetVector("_FillAmount", pos);
    }

    //https://forum.unity.com/threads/manually-calculate-angular-velocity-of-gameobject.289462/#post-4302796
    Vector3 GetAngularVelocity(Quaternion foreLastFrameRotation, Quaternion lastFrameRotation)
    {
        var q = lastFrameRotation * Quaternion.Inverse(foreLastFrameRotation);
        // no rotation?
        // You may want to increase this closer to 1 if you want to handle very small rotations.
        // Beware, if it is too close to one your answer will be Nan
        if (Mathf.Abs(q.w) > 1023.5f / 1024.0f)
            return Vector3.zero;
        float gain;
        // handle negatives, we could just flip it but this is faster
        if (q.w < 0.0f)
        {
            var angle = Mathf.Acos(-q.w);
            gain = -2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        else
        {
            var angle = Mathf.Acos(q.w);
            gain = 2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        Vector3 angularVelocity = new Vector3(q.x * gain, q.y * gain, q.z * gain);

        if (float.IsNaN(angularVelocity.z))
        {
            angularVelocity = Vector3.zero;
        }
        return angularVelocity;
    }

    float GetLowestPoint()
    {
        float lowestY = float.MaxValue;
        Vector3 lowestVert = Vector3.zero;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {

            Vector3 position = transform.TransformPoint(vertices[i]);

            if (position.y < lowestY)
            {
                lowestY = position.y;
                lowestVert = position;
            }
        }
        return lowestVert.y;
    }
    public IEnumerator UpdateLiquid(Liquid lv, LiquidProperties liquids, float speed)
    {
        Color avgColor = (lv.color + liquids.liquidColor) / 2;
        Color avgTopColor = (lv.topColor + liquids.topColor) / 2;
        float avgMaxWobble = (lv.MaxWobble + liquids.maxWobble) / 2;
        float avgThickness = (lv.Thickness + liquids.thickness) / 2;
        float avgFoamSmoothness = (lv.foamSmoothness + liquids.foamSmoothness) / 2;
        Color avgFoamColor = (lv.foamColor + liquids.foamColor) / 2;
        float avgFoamWidth = (lv.foamWidth + liquids.foamWidth) / 2;
        float avgRimPower = (lv.rimPower + liquids.rimPower) / 2;
        Color avgRimColor = (lv.rimColor + liquids.rimColor) / 2;

        float t = 0f;
        while (t < 1f)
        {
            lv.color = Color.Lerp(lv.color, avgColor, t);
            lv.topColor = Color.Lerp(lv.topColor, avgTopColor, t);
            lv.MaxWobble = Mathf.Lerp(lv.MaxWobble, avgMaxWobble, t);
            lv.Thickness = Mathf.Lerp(lv.Thickness, avgThickness, t);
            lv.foamSmoothness = Mathf.Lerp(lv.foamSmoothness, avgFoamSmoothness, t);
            lv.foamColor = Color.Lerp(lv.foamColor, avgFoamColor, t);
            lv.foamWidth = Mathf.Lerp(lv.foamWidth, avgFoamWidth, t);
            lv.rimPower = Mathf.Lerp(lv.rimPower, avgRimPower, t);
            lv.rimColor = Color.Lerp(lv.rimColor, avgRimColor, t);

            t += speed * Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set to exact averages
        lv.color = avgColor;
        lv.topColor = avgTopColor;
        lv.MaxWobble = avgMaxWobble;
        lv.Thickness = avgThickness;
        lv.foamSmoothness = avgFoamSmoothness;
        lv.foamColor = avgFoamColor;
        lv.foamWidth = avgFoamWidth;
        lv.rimPower = avgRimPower;
        lv.rimColor = avgRimColor;
    } 

    private void PotionCreator()
    {
        string potionName;
        if (fillAmount <= 0)
        {
            combos.stringLists.Clear();
            potionName = string.Empty;
        }
        if (combos.stringLists.Count >= 3 && combos.stringLists.Count <= 5)
        {
            string sortedCombos = string.Join(",", combos.stringLists.OrderBy(c => c));
            potionName = potionCombinations.FirstOrDefault(x => x.Value.OrderBy(y => y).SequenceEqual(sortedCombos.Split(','))).Key;
            if (!potions.stringLists.Contains(potionName))
            {
                potions.stringLists.Add(potionName);
                GameObject.Find("UnlockPotion").GetComponent<Animator>().Play("Congrats");
            }
        }
    }
}