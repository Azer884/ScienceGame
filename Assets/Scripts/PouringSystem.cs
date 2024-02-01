using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;
using UnityEditor.Animations;

public class PouringSystem : MonoBehaviour
{
    private LiquidVolume liquid;
    private float Multiplayer = .2f;
    private ParticleSystem particleSystem;


    // Start is called before the first frame update
    void Start()
    {
        liquid = gameObject.GetComponent<LiquidVolume>();
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Angle(Vector3.down, transform.parent.forward) <= 65f || Vector3.Angle(Vector3.down, transform.parent.right) <= 65f || Vector3.Angle(Vector3.down, -transform.parent.forward) <= 65f || Vector3.Angle(Vector3.down, -transform.parent.right) <= 65f)
        {
            liquid.level -= Multiplayer * Time.deltaTime;

            if (liquid.level <= 0.02f)
            {
                particleSystem.Stop();
            }
            else
            {
                particleSystem.Play();
            }
        }
        else
        {
            particleSystem.Stop();
        }
        if (liquid.level <= 0.02f)
        {
            liquid.alpha = 0f;
        }
        else
        {
            liquid.alpha = 1f;
        }
    }
}
