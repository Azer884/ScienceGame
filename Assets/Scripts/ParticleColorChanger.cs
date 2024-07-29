using UnityEngine;

public class ParticleColorChanger : MonoBehaviour
{
    // Start is called before the first frame update
    private new ParticleSystem particleSystem;
    private Liquid liquid;

    [System.Obsolete]
    void Awake()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
        liquid = gameObject.GetComponentInParent<Liquid>();
        particleSystem.startColor = liquid.color;
    }

    [System.Obsolete]
    void Update() {
        particleSystem.startColor = liquid.color;
    }
}
