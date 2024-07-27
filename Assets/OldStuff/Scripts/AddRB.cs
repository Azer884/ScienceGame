using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRB : MonoBehaviour
{
    private float timerDuration = 18.7f;
    private float timer;
    private bool done;
    // Start is called before the first frame update
    void Start()
    {
        timer = timerDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (!done)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                gameObject.AddComponent<Rigidbody>();
                gameObject.AddComponent<ObjectGrabbable>();
                done = true;
            }
        }
        
    }
}
