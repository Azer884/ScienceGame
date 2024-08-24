using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    [SerializeField]private PlayerStats stats;
    [SerializeField]private Animator block;

    public float currentHealth;
    public Scrollbar healthBar;
    private TeleportationController tp;
    public GameObject UI;
    public bool isDef;

    [Obsolete]
    private void Start()
    {
        currentHealth = stats.HP;
        tp = PlayerManager.Instance.player.GetComponent<TeleportationController>();
    }

    public void TakeDamage(float damage)
    {
        if (!isDef)
        {
            currentHealth -= damage;
            if (healthBar)
            {
                healthBar.size = currentHealth / 100f;
            }
        }
        else if(block)
        {
            block.Play("Congrats");
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (this == PlayerManager.Instance.player.GetComponent<HP>())
        {
            if (PlayerManager.Instance.player.GetComponent<Movement>().index == 15)
            {
                PlayerManager.Instance.player.GetComponent<HP>().currentHealth = 100f;
                PlayerManager.Instance.player.GetComponent<Movement>().index = 0;
            }
            UI.SetActive(true);
        }
        else
        {
            tp.enemyIsDead = true;
            Destroy(gameObject);
        }
    }
}