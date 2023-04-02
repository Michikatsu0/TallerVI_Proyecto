using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShot : MonoBehaviour
{

    public GameObject enemyProjectile2; //the objetc to instantiate
    public GameObject enemyProjectile;
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
            GameObject enemyprojectile2 = Instantiate(enemyProjectile2, transform.position+ new Vector3(0,1,0), Quaternion.identity);//instantiates the object 
            yield return new WaitForSeconds(shootTime);//cooldown time
            GameObject enemyProjectile = Instantiate(enemyProjectile2, transform.position + new Vector3(0, 1, 0), Quaternion.identity);//instantiates the object 
            yield return new WaitForSeconds(shootTime);//cooldown time
        }
    }
}
