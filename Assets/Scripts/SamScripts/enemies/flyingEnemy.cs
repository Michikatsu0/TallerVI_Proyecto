using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingEnemy : MonoBehaviour
{
    [SerializeField] float Xdurationx = 1f;
    [SerializeField] Ease Xeasex = Ease.Linear;
    [SerializeField] int Xpositionx = 0;
    [SerializeField] int xposition2 = 0;
    [SerializeField] int Yposition1 = 0;
    [SerializeField] int Yposition2 = 0;
    [SerializeField] float yDuration = 0;



    [SerializeField] float DestroyTime; //time for destruction
    int DamageDealt = -1; //damage done to the player

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(); //initialize dotween
        enemymovement();//calls the enemy movement. Dotween does not need to be in update.
        vertical1();

    }
    void enemymovement()
    {
        transform.DOMoveX(Xpositionx, Xdurationx).SetEase(Xeasex).OnComplete(enemyRotation);
    }
    void enemymovement2()
    {
        transform.DOMoveX(xposition2, Xdurationx).SetEase(Xeasex).OnComplete(enemyrotation2);
    }
    void enemyRotation()
    {
        transform.DORotate(new Vector3(-90, 270, 0), 0.3f).OnComplete(enemymovement2);
    }
    void enemyrotation2()
    {
        transform.DORotate(new Vector3(-90, 90, 0), 0.3f).OnComplete(enemymovement);
    }
    void vertical1 ()
    {
        transform.DOMoveY(Yposition1, yDuration).SetEase(Ease.Linear).OnComplete(vertical2);
    }
    private void vertical2()
    {
        transform.DOMoveY(Yposition2, yDuration).SetEase(Ease.Linear).OnComplete(vertical1);

    }
    void OnTriggerEnter(Collider other)
    {
        Health player = other.gameObject.GetComponent<Health>(); //When Colliding, accesses the health component
        if (player != null) //compares if is colliding with the player
        {
            if (Health.IsInvincible == false) //only damages if the player is not invencible
            {
                Health.IsInvincible = true;
                player.ChangeHealth(DamageDealt); //decreases the health by 1
            }
        }
    }
}
