using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] private ScifiCoinSettings coinSettings;
    [SerializeField] private GameObject scifiMesh;
    [SerializeField] public float delayToFollow, followSpeed;
    public bool inEnemy;

    private float inEnemyTime;
    private int amount;
    private GameObject playerPosition;
    private AudioSource audioSource;
    private Rigidbody rgbd;
    private Collider col;

    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
        rgbd.useGravity = false;
        rgbd.isKinematic = true;
        col = GetComponent<Collider>();
        col.isTrigger = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 0.5f;
        audioSource.playOnAwake = false;
        playerPosition = GameObject.Find("Player_Armature_CharacterController");

        if (inEnemy)
        {
            rgbd.useGravity = true;
            rgbd.isKinematic = false;
        }
    }

    void Update()
    {
        if (inEnemy)
        {
            inEnemyTime += Time.deltaTime;
            if (inEnemyTime >= delayToFollow)
            {
                rgbd.useGravity = false;
                rgbd.isKinematic = true;
                rgbd.position = Vector3.MoveTowards(transform.position, playerPosition.transform.position, followSpeed * Time.deltaTime);
                
            }
            return;
        }

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
            audioSource.PlayOneShot(coinSettings.coinClips[Random.Range(0,coinSettings.coinClips.Count)], 1f);
            upgradesManager.CoinQuantity += amount;
            UpgradeCoinUI.Instance.changeCoinText();
            scifiMesh.SetActive(false);
            col.enabled = false;
            Invoke(nameof(DelayDeath), 1f);
        }
    }

    void DelayDeath()
    {
        Destroy(gameObject);
    }
}
