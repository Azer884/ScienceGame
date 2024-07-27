using UnityEngine;

public class LiquidPour : MonoBehaviour
{
    private Quaternion OriginalRotation;
    private float fillSpeed = 0.01f;
    private Liquid lv;
    private float yMax;
    private float yMin;

    public static LiquidPropertiesCollection liquids;

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
                UpdateLiquidColor();
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

    void UpdateLiquidColor()
    {
        // Assuming the Liquid script has a Color property for the liquid color
        Color currentColor = lv.color;
        lv.color = Color.Lerp(currentColor, liquids.liquidPropertiesList[0].liquidColor, fillSpeed);
    }
}
