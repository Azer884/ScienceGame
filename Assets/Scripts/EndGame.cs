using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public Outline[] outline;
    public GameObject BlackScene;
    void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < outline.Length; i++)
        {
            if (outline[i].enabled && outline[i].OutlineColor == Color.green)
            {
                StartCoroutine(LoadNextScene());
                break;
            }
        }
        
    }
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1f);
        BlackScene.SetActive(true);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
