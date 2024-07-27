using UnityEngine;

public class LiquidPour : MonoBehaviour
{
    private Quaternion OriginalRotation;
    private float fillSpeed = 0.01f;
    private Liquid lv;
    private float yMax;
    private float yMin;

    public LiquidPropertiesCollection liquids;

    // Start is called before the first frame update
    void Start()
    {
        lv = transform.parent.GetComponent<Liquid>();
        OriginalRotation = transform.localRotation;
        yMax = transform.parent.Find("TopLevel").localPosition.y;
        yMin = transform.parent.Find("BottomLevel").localPosition.y;
    }

    private void Update()
    {
        transform.SetLocalPositionAndRotation(Vector3.up * WaterCheckPos(lv, yMax, yMin), OriginalRotation);
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Water"))
        {
            if (lv.fillAmount < 1f)
            {
                lv.fillAmount += fillSpeed;
                UpdateLiquid(lv, liquids.liquidPropertiesList[0], fillSpeed);
            }
            if (lv.fillAmount >= 1f)
            {
                transform.localRotation = Quaternion.Euler(Random.value * 30 - 15, Random.value * 30 - 15, Random.value * 30 - 15);
            }
            else
            {
                transform.localRotation = OriginalRotation;
            }
        }
    }

    float WaterCheckPos(Liquid lv, float yMax, float yMin)
    {
        float y;
        y = yMin + (yMax - yMin) * lv.fillAmount;
        return y;
    }

    public void UpdateLiquid(Liquid lv, LiquidProperties liquids, float speed)
    {
        // Assuming the Liquid script has a Color property for the liquid color
        Color currentColor = lv.color;
        lv.color = Color.Lerp(currentColor, liquids.liquidColor, speed);
        lv.topColor = Color.Lerp(currentColor, liquids.topColor, speed);
        lv.MaxWobble = Mathf.Lerp(lv.MaxWobble, liquids.maxWobble, speed);
        lv.Thickness = Mathf.Lerp(lv.Thickness,liquids.thickness, speed);
        lv.foamSmoothness = Mathf.Lerp(lv.foamSmoothness,liquids.foamSmoothness, speed);
        lv.foamColor = Color.Lerp(lv.foamColor, liquids.foamColor, speed);
        lv.foamWidth = Mathf.Lerp(lv.foamWidth,liquids.foamWidth, speed);
        lv.rimPower = Mathf.Lerp(lv.rimPower,liquids.rimPower, speed);
        lv.rimColor = Color.Lerp(lv.rimColor, liquids.rimColor, speed);
    }
}
