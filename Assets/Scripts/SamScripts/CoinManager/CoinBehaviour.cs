using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] private ScifiCoinSettings coinSettings;
    [SerializeField] private float speed;
    private GameObject playerPosition;

    public bool inEnemy;
    private int amount;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerPosition = GameObject.Find("Player_Armature_CharacterController");
    }

    void Update()
    {
        if (inEnemy)
            transform.position = Vector3.MoveTowards(transform.position, playerPosition.transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player"))
        {
            if (inEnemy)
                amount = Random.Range(50, 81);
            else
                amount = Random.Range(5, 21);
            audioSource.PlayOneShot(coinSettings.coinClips[Random.Range(0,coinSettings.coinClips.Count)], 0.5f);
            upgradesManager.CoinQuantity += amount;
            UpgradeCoinUI.Instance.changeCoinText();
            Destroy(gameObject); //pool object
        }
    }
}
