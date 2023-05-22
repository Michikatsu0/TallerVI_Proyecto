using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] GameObject scfiCoin;
    [SerializeField] int lowerLimit;
    [SerializeField] int upperLimit;
    int coinsSpawned;

    private void OnDestroy()
    {   coinsSpawned = Random.Range(lowerLimit, upperLimit);
        for (int i = 0; i < coinsSpawned; i++)
        {
            GameObject CoinSpawner = Instantiate(scfiCoin, transform.position + new Vector3(i,0,0), Quaternion.identity);
        }   
    }
}
