using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth; //max health, 3
    [SerializeField] int health { get => health; set => health = currentHealth; }
    public static int currentHealth; //Is public so the EnemyDamage can access it
    //invencibilidad
    [SerializeField] int maxInvincible; //time for invencibility
    public static bool IsInvincible;
    [SerializeField] bool invencible { get => invencible; set => invencible = IsInvincible; }
    Rigidbody rigidbody;
    public int deathTimer; //time for the destruction of the players



    void Start()
    {
        IsInvincible = false;
        currentHealth = maxHealth; //makes sure the health is at max
        rigidbody = GetComponent<Rigidbody>();
    }
    void Update()
    {
        DestroyPlayer();     //Reads current health every frame, so the player is destroyed automatically when its 0       
    }
    public void ChangeHealth(int amount) //Changes the current Health, public so enemydamage can access it. When damaged, starts the timer for invencibility
    {
        StartCoroutine(InvencibleCoroutine());
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        //Debug.Log("health changed ");

    }
    void DestroyPlayer() //Destroys the player when the life is 0
    {
        if (currentHealth == 0)
        {
            StartCoroutine(DeathCoroutine());

        }
    }

    IEnumerator InvencibleCoroutine() //the invencibility timer, after waiting, sets the invencibility for false so the player can be damaged
    {
        yield return new WaitForSeconds(maxInvincible);
        IsInvincible = false;
    }
    IEnumerator DeathCoroutine() //waits for the destruction of the player, use and adjust the time for a death animation
    {
        yield return new WaitForSeconds(deathTimer);

        Destroy(gameObject);


    }
}
