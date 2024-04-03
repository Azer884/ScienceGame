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
        DontDestroyOnLoad(gameObject);
        timer = timerDuration;
    }

    // Update is called once per frame
    void Update()
    {
        FootSFX();
        SinkOpenning();
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
    void SinkOpenning()
    {
        if(sink.IsOpened)
        {
            sink.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            sink.transform.GetChild(0).gameObject.SetActive(false);
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
