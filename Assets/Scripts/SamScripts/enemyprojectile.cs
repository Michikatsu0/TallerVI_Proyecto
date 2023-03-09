using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// the projectiles, uses dotween to move and recycles code from enemydamage
/// </summary>

public class enemyprojectile : MonoBehaviour
{
    [SerializeField] float DestroyTime; //time for destruction
    int DamageDealt = -1; //damage done to the player
    
    //just duration of the movement
    /*[SerializeField] float Xdurationx = 1f;
    [SerializeField] float Ydurationy = 1f;
    [SerializeField] float Zdurationz = 1f;*/
    [SerializeField] float movementDuration = 1f;


    //if it moves in given direction
    [SerializeField] bool Xmovex = true;
    [SerializeField] bool Ymovey = true;
    [SerializeField] bool Zmovez = true;

    //final position
    [SerializeField] int Xpositionx = 0;
    [SerializeField] int Ypositiony = 0;
    [SerializeField] int Zpositionz = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (Xmovex)
        {
            DOTween.Sequence().Append(transform.DOMoveX(Xpositionx, movementDuration)).OnComplete(Destroy);
        }//the movement itself with above values. the proyectile destroys itself after movement is completed
        if (Ymovey)
        {
            DOTween.Sequence().Append(transform.DOMoveY(Ypositiony, movementDuration)).OnComplete(Destroy);
        }
        if (Zmovez)
        {
            DOTween.Sequence().Append(transform.DOMoveZ(Zpositionz, movementDuration)).OnComplete(Destroy);
        }

    }
    private void Destroy()
    {
        if (gameObject !=null)
        {
            Destroy(gameObject);
        }
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
