using UnityEngine;

public class PouringSystem : MonoBehaviour
{
    [HideInInspector]public Liquid liquid;
    private new ParticleSystem particleSystem;


    // Start is called before the first frame update
    void Start()
    {
        liquid = gameObject.GetComponent<Liquid>();
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (liquid.fillAmount < 0.001f)
        {
            liquid.fillAmount = 0f;
        }
        if (Vector3.Angle(Vector3.down, transform.parent.forward) <= 55f || Vector3.Angle(Vector3.down, transform.parent.right) <= 55f || Vector3.Angle(Vector3.down, -transform.parent.forward) <= 55f || Vector3.Angle(Vector3.down, -transform.parent.right) <= 55f)
        {
            if(liquid.fillAmount > 0.15f)
            {
                liquid.fillAmount -= Time.deltaTime * liquid.fillAmount * 0.75f;
            }
            else
            {
                liquid.fillAmount -= Time.deltaTime * (liquid.fillAmount + .15f) * 0.75f;
            }
            

            if (liquid.fillAmount <= 0.02f)
            {
                particleSystem.gameObject.SetActive(false);
            }
            else
            {
                particleSystem.gameObject.SetActive(true);
            }
        }
        else
        {
            particleSystem.gameObject.SetActive(false);
        }
    }
}
