using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationController : MonoBehaviour
{
    private TimeManager timeManager;
    private GameManager gameManager;
    public Liquid liquid;
    public bool isInArena;
    public bool enemyIsDead;
    private bool isCoroutineRunning;

    [System.Obsolete]
    private void Awake() {
        timeManager = FindObjectOfType<TimeManager>();
        gameManager = FindObjectOfType<GameManager>();
        gameManager.enabled = true;
    }

    [System.Obsolete]
    private void Update()
    {
        liquid = FindObjectOfType<Liquid>();
        if ((timeManager.Hours >= 23 || liquid.potionCreated) && !isInArena && !isCoroutineRunning)
        {
            StartCoroutine(Wait(5f));
        }
        else if ((timeManager.Hours >= 6 && timeManager.Hours < 7 || enemyIsDead) && isInArena)
        {
            OnTeleportBackFromArenaButtonClicked();
        }   
    }

    private void OnTeleportToArenaButtonClicked()
    {
        timeManager.TeleportToArena();
        // Add your scene loading logic here if necessary
        SceneManager.LoadScene("Arena");
        isInArena = true;
        gameManager.transform.GetChild(0).gameObject.SetActive(true);
        isCoroutineRunning = false; // Reset the flag once teleportation is done
    }

    private void OnTeleportBackFromArenaButtonClicked()
    {
        timeManager.TeleportBackFromArena();
        // Add your scene loading logic here if necessary
        SceneManager.LoadScene("Loading"); // Replace "MainScene" with your main scene name
        isInArena = false;
        enemyIsDead = false;
        gameManager.transform.GetChild(0).gameObject.SetActive(false);
    }

    private IEnumerator Wait(float delay)
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(delay);
        OnTeleportToArenaButtonClicked();
    }
}
