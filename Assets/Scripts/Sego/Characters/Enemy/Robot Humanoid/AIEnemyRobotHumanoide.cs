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
    private Animator animator;
    private Vector3 searchTargetPos;
    private float currentVelocity, currentDistance, searchTime;


    void Start()
    {
        playerTarget = GameObject.Find("Player_Armature_CharacterController").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.magnitude == 0)
            animator.SetBool("IsMove", false);
        else
            animator.SetBool("IsMoving", true);

        animator.SetFloat("ZMove", agent.velocity.magnitude);

        if (onAlert)
        {
            agent.SetDestination(searchTarget.position);
            agent.stoppingDistance = 0;
        }
        else
        {
            searchTime += Time.deltaTime;
            agent.stoppingDistance = 0;
            if (searchTime >= baseEnemySettings.timeToSearchPos)
            {
                searchTime = 0;
                searchTargetPos.y = transform.position.y;
                var direction = Random.Range(0, 2);

                if (direction == 0)
                    searchTargetPos.z = transform.position.z + UnityEngine.Random.Range(-baseEnemySettings.alertDistance, -5.0f);
                else
                    searchTargetPos.z = transform.position.z + UnityEngine.Random.Range(5.0f, baseEnemySettings.alertDistance);

                searchTargetPos.x = 0;

                searchTarget.position = Vector3.Lerp(searchTarget.position, searchTargetPos, baseEnemySettings.lerpSearchPosTarget * Time.deltaTime * 2f);
                
                agent.SetDestination(searchTarget.position);
            }
        }
    }
}
