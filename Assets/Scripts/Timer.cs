using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timerDuration;
    private float timer;
    public GameObject gameOver;
    public TextMeshProUGUI timeLeft;
    public float timeStartAt;
    // Start is called before the first frame update
    void Start()
    {
        timer = timerDuration;
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            gameOver.SetActive(true);
            StartCoroutine(LoadNextScene());
        }
        if (timer <= timeStartAt)
        {
            timeLeft.text = ((int)timer).ToString();
            if (timer <= 30f && timer > 15f)
            {
                timeLeft.color = Color.yellow;                
            }
            else if (timer <= 15f)
            {
                timeLeft.color = Color.red;
            }
        }
    }
    IEnumerator LoadNextScene()
    {
        gameOver.SetActive(true);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(0);
    }
}
