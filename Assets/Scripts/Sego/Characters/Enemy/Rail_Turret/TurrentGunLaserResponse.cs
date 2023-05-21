using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TurrentGunLaserResponse : BaseEnemyController
{
    [SerializeField] private TurretGunLaserSettings turretGunSettings;
    [SerializeField] private Rig playerAimRig;
    [SerializeField] private Rig searchAimRig;
    [SerializeField] private Transform searchTarget;
    private Vector3 multiAimOffset, searchTargetPos;
    private float searchTime;

    void Start()
    {
        searchTargetPos.z = transform.position.z;
    }

    private void Update()
    {
        TimingDistanceAlert();
        if (onAlert)
        {
            turretGunSettings.emissionMaterial.SetColor("_EmissionColor", turretGunSettings.turrentColors[1]);
            searchAimRig.weight = Mathf.Lerp(searchAimRig.weight, 0f, turretGunSettings.lerpAimWeight * Time.deltaTime);
            playerAimRig.weight = Mathf.Lerp(playerAimRig.weight, 1f, turretGunSettings.lerpAimWeight * Time.deltaTime);


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

            searchTarget.position = Vector3.Lerp(searchTarget.position, searchTargetPos, baseEnemySettings.lerpSearchPosTarget * Time.deltaTime);
        }
    }

}

