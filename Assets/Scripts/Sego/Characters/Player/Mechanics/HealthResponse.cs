using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class HealthResponse : MonoBehaviour
{
    [SerializeField] private StatsSettings statsSettings;

    public int currentHealth;
    public bool IsInvincible = false;

    private void Start()
    {
        currentHealth = statsSettings.maxHealth;
    }

    private void Update()
    {
        IsDeath();
    }

    public void ChangeHealth(int amount) //Changes the current Health, public so enemydamage can access it. When damaged, starts the timer for invencibility
    {
        StartCoroutine(InvencibleCoroutine());
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, statsSettings.maxHealth);
    }

    IEnumerator InvencibleCoroutine() //the invencibility timer, after waiting, sets the invencibility for false so the player can be damaged
    {
        yield return new WaitForSeconds(statsSettings.maxTimeInvincible);
        IsInvincible = false;
    }

    public void IsDeath()
    {
        if (currentHealth <= 0)
        {
            StartCoroutine(DeathCoroutine());
        }

        if (transform.position.y <= -15)
        {
            currentHealth = 0;
        }
    }

    IEnumerator DeathCoroutine() //waits for the destruction of the player, use and adjust the time for a death animation
    {
        LevelUIManager.Instance.lose = true;
        yield return new WaitForSeconds(statsSettings.deathTime);
        gameObject.SetActive(false);

    }

}

