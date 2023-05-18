using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// a script that acts as the turret, every "shoot time" seconds, instantiates an enemy projectile
/// </summary>

public class enemyshot : MonoBehaviour
{
    public GameObject enemyProjectile; //the objetc to instantiate
    [SerializeField] float shootTime; //the time between shoots, in seconds.

        
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(shoot()); //starts the cicle to shoot infinitely
    }
    IEnumerator shoot()
    {
        while (true)
        {
            GameObject enemyprojectile = Instantiate(enemyProjectile, transform.position, Quaternion.identity);//instantiates the object 
            yield return new WaitForSeconds(shootTime);//cooldown time
        }
    }
}
