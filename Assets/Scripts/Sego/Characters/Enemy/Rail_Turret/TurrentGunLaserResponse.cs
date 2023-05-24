using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TurrentGunLaserResponse : BaseEnemyController
{
    [SerializeField] TurretGunLaserSettings turretGunSettings;

    private Rig playerAimRig;
    private Rig searchAimRig;
    private Transform searchTarget;
    private Vector3 multiAimOffset, searchTargetPos;
    private float searchTime;

    void Start()
    {
        
        searchTargetPos.z = transform.position.z;
        playerTarget = GameObject.Find("Player_Armature_CharacterController").GetComponent<Transform>();
        searchTarget = GameObject.Find("Search_Target_Turret").transform;
        playerAimRig = GameObject.Find("MultiAim_Player_Turret").GetComponent<Rig>();
        searchAimRig = GameObject.Find("MultiAim_Search_Turret").GetComponent<Rig>();
    }

    private void Update()
    {
        TimingDistanceAlert();
        if (onAlert)
        {
            turretGunSettings.emissionColor.SetColor("_EmissionColor", turretGunSettings.turrentColors[1]);
            searchAimRig.weight = Mathf.Lerp(searchAimRig.weight, 0f, turretGunSettings.lerpAimWeight * Time.deltaTime);
            playerAimRig.weight = Mathf.Lerp(playerAimRig.weight, 1f, turretGunSettings.lerpAimWeight * Time.deltaTime);


        }
        else
        {
            turretGunSettings.emissionColor.SetColor("_EmissionColor", turretGunSettings.turrentColors[0]);
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

