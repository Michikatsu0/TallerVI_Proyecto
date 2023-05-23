using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] float speed;
     GameObject playerPosition;
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = GameObject.Find("Player_Armature_CharacterController");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(3,0,0);
        transform.position = Vector3.MoveTowards(transform.position, playerPosition.transform.position, speed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        if (target.CompareTag("Player")) {
            upgradesManager.CoinQuantity++;
            Destroy(gameObject);
        }
    }
}
