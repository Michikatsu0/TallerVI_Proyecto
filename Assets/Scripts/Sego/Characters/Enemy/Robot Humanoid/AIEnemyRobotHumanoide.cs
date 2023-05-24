using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class AIEnemyRobotHumanoide : BaseEnemyController
{
    [SerializeField] private Transform searchTarget;
    private NavMeshAgent agent;
    private Vector3 searchTargetPos;
    private float currentVelocity, currentDistance, searchTime;
     // Start is called before the first frame update
    void Start()
    {
        playerTarget = GameObject.Find("Player_Armature_CharacterController").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onAlert)
        {
            agent.SetDestination(searchTarget.position);
        }
        else
        {
            searchTime += Time.deltaTime;

            if (searchTime >= baseEnemySettings.timeToSearchPos)
            {
                searchTime = 0;
                searchTargetPos.y = transform.position.y;
                searchTargetPos.z = transform.position.z + UnityEngine.Random.Range(-baseEnemySettings.alertDistance, baseEnemySettings.alertDistance);

                searchTarget.position = Vector3.Lerp(searchTarget.position, searchTargetPos, baseEnemySettings.lerpSearchPosTarget * Time.deltaTime * 2f);
                
                agent.SetDestination(searchTarget.position);
            }
        }
    }
}
