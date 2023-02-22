using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] float Speed; //movement speed
    Rigidbody rigidbody;
    [SerializeField] float DestroyTime; //time for destruction
    int Direction = 1;//moves positive in the x axis if positive.
    int DamageDealt = -1;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Vector3 position = rigidbody.position; //Moves the enemy in the X axis. Only for test purpuses.
        position.x = position.x + Time.deltaTime * Speed * Direction;//change the speed to 0 so it doesnt move
        rigidbody.MovePosition(position);

    }
    void OnTriggerEnter(Collider other)
    {
        Health player = other.gameObject.GetComponent<Health>(); //When Colliding, accesses the health component
        if (player != null) //compares if is colliding with the player
        {
            StartCoroutine(SelfDestruct()); //after colliding, starts the destruction sentence
            if (Health.IsInvincible == false) //only damages if the player is not invencible
            {
                Health.IsInvincible = true;
                player.ChangeHealth(DamageDealt); //decreases the health by 1
            }
        }
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);//destroys the object
    }
}

