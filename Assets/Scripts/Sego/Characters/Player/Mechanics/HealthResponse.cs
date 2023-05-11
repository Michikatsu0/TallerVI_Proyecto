using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HealthResponse : MonoBehaviour
{
    [SerializeField] private StatsSettings statsSettings;

    public int currentHealth;

    private RagdollResponse ragdoll;
    private CharacterController characterController;
    private Slider healthSlider;

    private void Start()
    {
        healthSlider = GameObject.Find("Health Bar").GetComponent<Slider>();

        ragdoll = GetComponentInChildren<RagdollResponse>();
        characterController = GetComponent<CharacterController>();
        currentHealth = statsSettings.maxHealth;

        var rigidBodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidBody in rigidBodies)
        {
            HitboxResponse hitbox = rigidBody.gameObject.AddComponent<HitboxResponse>();
            hitbox.healthResponse = this;
        }

    }

    private void Update()
    {
        IsDeath();
    }

    public void TakeDamage(int amount) //Changes the current Health, public so enemydamage can access it. When damaged, starts the timer for invencibility
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, statsSettings.maxHealth);
    }


    public void IsDeath()
    {
        if (currentHealth <= 0.0f)
        {
            StartCoroutine(DeathCoroutine());
        }

        if (transform.position.y <= -20)
        {
            currentHealth = 0;
        }
    }

    IEnumerator DeathCoroutine() //waits for the destruction of the player, use and adjust the time for a death animation
    {
        ragdoll.ActivateRagdolls();
        characterController.enabled = false;
        LevelUIManager.Instance.lose = true;
        yield return new WaitForSeconds(statsSettings.deathTime);
    }

}

