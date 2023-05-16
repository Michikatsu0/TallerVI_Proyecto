using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickUpCoins : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("SciFi-Coin")) 
        { Debug.Log("please be working"); }
    }
}
