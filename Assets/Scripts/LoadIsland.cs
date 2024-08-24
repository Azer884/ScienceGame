using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadIsland : MonoBehaviour
{
    // Start is called before the first frame update
    public void Start()
    {
        SceneManager.LoadScene("Island");
    }
}
