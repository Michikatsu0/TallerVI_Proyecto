using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject playerPosition;

    public bool inEnemy;

    void Start()
    {
        playerPosition = GameObject.Find("Player_Armature_CharacterController");
    }

    void Update()
    {
        if (inEnemy)
            transform.position = Vector3.MoveTowards(transform.position, playerPosition.transform.position, speed * Time.deltaTime);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        if (target.CompareTag("Player")) {
            upgradesManager.CoinQuantity++;
            Destroy(gameObject); //pool object
        }
    }
}
