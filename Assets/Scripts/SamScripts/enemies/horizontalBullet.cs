using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horizontalBullet : MonoBehaviour
{
    [SerializeField] float DestroyTime; //time for destruction
    [SerializeField] int xmovement;
    int DamageDealt = -1; //damage done to the player

    public GameObject player;
    // Start is called before the first frame update
    private void Start()
    {
        transform.DOMoveX(xmovement, 1).SetEase(Ease.Linear).OnComplete(OnDestroy);

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
    private void OnCollisionEnter(Collision collision)
    {
        Health player = collision.gameObject.GetComponent<Health>(); //When Colliding, accesses the health component
        GameObject target = collision.gameObject;

        if (target.CompareTag("Player")) //compares if is colliding with the player
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
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
