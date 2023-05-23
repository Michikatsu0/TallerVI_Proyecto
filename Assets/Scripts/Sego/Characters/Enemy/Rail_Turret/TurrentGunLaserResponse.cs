using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;

public class TurrentGunLaserResponse : BaseEnemyController
{
    [SerializeField] private TurretGunLaserSettings turretGunSettings;
    [SerializeField] private Rig playerAimRig;
    [SerializeField] private Rig searchAimRig;
    [SerializeField] private Transform searchTarget;
    private Vector3 searchTargetPos;
    private float searchTime;
    private bool flagOnDeath = true;

    private HealthEnemyResponse healthEnemyResponse;
    private Rigidbody searchRigidbody;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 0.5f;
        audioSource.volume = 0.25f;
        searchTargetPos.z = transform.position.z;
        searchRigidbody = searchTarget.gameObject.AddComponent<Rigidbody>();
        searchRigidbody.useGravity = false; 
        searchRigidbody.isKinematic = true;
        healthEnemyResponse = GetComponent<HealthEnemyResponse>();
    }

    private void Update()
    {

        if (healthEnemyResponse.currentHealth <= 0)
        {
            searchAimRig.weight = Mathf.Lerp(searchAimRig.weight, 1f, turretGunSettings.lerpAimWeight * Time.deltaTime);
            playerAimRig.weight = Mathf.Lerp(playerAimRig.weight, 0f, turretGunSettings.lerpAimWeight * Time.deltaTime);
            onAlert = false;
            if (flagOnDeath)
            {
                flagOnDeath = false;
                searchTargetPos.y = playerTarget.position.y - 0.7f;
                searchTargetPos.z = playerTarget.position.z;
            }
            searchRigidbody.MovePosition(Vector3.Lerp(searchTarget.position, searchTargetPos, baseEnemySettings.lerpSearchPosTarget * Time.deltaTime * 10f));

            audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, 0.1f * Time.deltaTime * 2f);
            Invoke(nameof(DelayAimDeath), 1f);
            return;
        }

        TimingDistanceAlert();
        if (onAlert)
        {
            turretGunSettings.emissionMaterial.SetColor("_EmissionColor", turretGunSettings.turrentColors[1]);
            searchAimRig.weight = Mathf.Lerp(searchAimRig.weight, 0f, turretGunSettings.lerpAimWeight * Time.deltaTime);
            playerAimRig.weight = Mathf.Lerp(playerAimRig.weight, 1f, turretGunSettings.lerpAimWeight * Time.deltaTime);
            searchTargetPos.y = playerTarget.position.y - 0.7f;
            searchTargetPos.z = playerTarget.position.z;
            searchRigidbody.MovePosition(Vector3.Lerp(searchTarget.position, searchTargetPos, baseEnemySettings.lerpSearchPosTarget * Time.deltaTime * 10f));

        }
        else
        {
            turretGunSettings.emissionMaterial.SetColor("_EmissionColor", turretGunSettings.turrentColors[0]);
            searchAimRig.weight = Mathf.Lerp(searchAimRig.weight, 1f, turretGunSettings.lerpAimWeight * Time.deltaTime);
            playerAimRig.weight = Mathf.Lerp(playerAimRig.weight, 0f, turretGunSettings.lerpAimWeight * Time.deltaTime);
            searchTime += Time.deltaTime;

            if (searchTime >= baseEnemySettings.timeToSearchPos)
            {
                searchTime = 0;
                searchTargetPos.y = transform.position.y + UnityEngine.Random.Range(0.0f, baseEnemySettings.searchYlimit);
                searchTargetPos.z = transform.position.z + UnityEngine.Random.Range(-baseEnemySettings.alertDistance, baseEnemySettings.alertDistance);
            }

            searchRigidbody.MovePosition(Vector3.Lerp(searchTarget.position, searchTargetPos, baseEnemySettings.lerpSearchPosTarget * Time.deltaTime* 2f));
        }
    }

    void DelayAimDeath()
    {
        turretGunSettings.emissionMaterial.SetColor("_EmissionColor", Color.black);
    } 

}

