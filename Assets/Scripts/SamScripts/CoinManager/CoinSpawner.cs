using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] GameObject scifiCoin;
    [SerializeField] int lowerLimit;
    [SerializeField] int upperLimit;
    int coinsNumber;

    public void SpawnCoins()
    {
        coinsNumber = Random.Range(lowerLimit, upperLimit);
        for (int i = 0; i < coinsNumber; i++)
        {
            Vector2 ramdonPointSphere = Random.insideUnitCircle / 2;
            GameObject coinSpawner = Instantiate(scifiCoin, transform.position + (Vector3)ramdonPointSphere, Quaternion.identity);
            coinSpawner.GetComponent<CoinBehaviour>().inEnemy = true;
        }
    }
}
