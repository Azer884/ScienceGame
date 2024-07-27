using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public GameObject FootSteps;
    public GameObject ChairSfx;
    public Rigidbody rb;
    public Sink sink;
    private float timerDuration = 18.7f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        FootSteps.SetActive(false);
        sink.transform.GetChild(0).gameObject.SetActive(false);
        ChairSfx.SetActive(false);
        timer = timerDuration;
    }

    // Update is called once per frame
    void Update()
    {
        FootSFX();
        ChairSFX();
    }

    void FootSFX()
    {
        timer -= Time.deltaTime;

        if (rb.velocity.magnitude > 0.1f && timer <= 0f)
        {
            FootSteps.SetActive(true);
        }
        else
        {
            FootSteps.SetActive(false);
        }
    }
    
    void ChairSFX()
    {
        if (ChairSfx.transform.parent.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
        {
            ChairSfx.SetActive(true);
        }
        else
        {
            ChairSfx.SetActive(false);
        }
    }
}
