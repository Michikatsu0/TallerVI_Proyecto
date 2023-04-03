using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectile : MonoBehaviour
{
    [SerializeField] float DestroyTime; //time for destruction
    int DamageDealt = -1; //damage done to the player

    public GameObject player;

    //just duration of the movement
    /*[SerializeField] float Xdurationx = 1f;
    [SerializeField] float Ydurationy = 1f;
    [SerializeField] float Zdurationz = 1f;*/
    [SerializeField] float movementDuration = 1f;


    //if it moves in given direction
    [SerializeField] bool Xmovex = true;
    [SerializeField] bool Ymovey = true;
    [SerializeField] bool Zmovez = true;

    //If the proyectile is shot toward the player
    [SerializeField] bool aimPlayer = false;


    //final position
    [SerializeField] int Xpositionx = 0;
    [SerializeField] int Ypositiony = 0;
    [SerializeField] int Zpositionz = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (aimPlayer == true)
        {
            aimAtPlayerShot();
        }
        else { normalShot(); }
    }
    private void Destroy()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
    private void normalShot()//shots in the directon you choose
    {
        if (Xmovex)
        {
            DOTween.Sequence().Append(transform.DOMoveX(transform.position.x + Xpositionx, movementDuration)).OnComplete(Destroy);
        }//the movement itself with above values. the proyectile destroys itself after movement is completed
        if (Ymovey)
        {
            DOTween.Sequence().Append(transform.DOMoveY(transform.position.y + Ypositiony, movementDuration)).OnComplete(Destroy);
        }
        if (Zmovez)
        {
            DOTween.Sequence().Append(transform.DOMoveZ(transform.position.z + Zpositionz, movementDuration)).OnComplete(Destroy);
        }
    }
    private void aimAtPlayerShot()//shots at player
    {
        if (Xmovex)
        {
            DOTween.Sequence().Append(transform.DOMoveX(player.transform.position.x, movementDuration)).OnComplete(Destroy);
        }//the movement itself with above values. the proyectile destroys itself after movement is completed
        if (Ymovey)
        {
            DOTween.Sequence().Append(transform.DOMoveY(player.transform.position.y, movementDuration)).OnComplete(Destroy);
        }
        if (Zmovez)
        {
            DOTween.Sequence().Append(transform.DOMoveZ(player.transform.position.z, movementDuration)).OnComplete(Destroy);
        }

    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(DestroyTime);
        Destroy(gameObject);//destroys the object

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
}
