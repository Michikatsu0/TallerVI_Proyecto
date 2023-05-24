using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemyRobotHumanoide : BaseEnemyController
{
    [SerializeField] private Transform searchTarget;
    [SerializeField] private float stopDistances;
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 searchTargetPos;
    private float searchTime, currentVolume;
    private HealthEnemyResponse healthEnemyResponse;

    void Start()
    {
        healthEnemyResponse = GetComponent<HealthEnemyResponse>();
        playerTarget = GameObject.Find("Player_Armature_CharacterController").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        TimingDistanceAlert();

        if (agent.velocity.magnitude == 0)
        {
            animator.SetBool("IsMoving", false);
        }
        else
            animator.SetBool("IsMoving", true);

        animator.SetFloat("MoveZ", agent.velocity.magnitude);

        if (healthEnemyResponse.currentHealth <= 0) return;

        if (onAlert)
        {
            agent.SetDestination(playerTarget.position);
            agent.stoppingDistance = stopDistances;
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
                    searchTargetPos.z = transform.position.z + UnityEngine.Random.Range(baseEnemySettings.alertDistance, -8.0f);
                else
                    searchTargetPos.z = transform.position.z + UnityEngine.Random.Range(8.0f, baseEnemySettings.alertDistance);

                searchTargetPos.x = 0;

                searchTarget.position = Vector3.Lerp(searchTarget.position, searchTargetPos, baseEnemySettings.lerpSearchPosTarget * Time.deltaTime * 2f);
                
                agent.SetDestination(searchTarget.position);
            }
        }
    }

    private void OnFootStep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (healthEnemyResponse.statsEnemySettings.footClips.Count > 0)
            {
                var index = Random.Range(0, healthEnemyResponse.statsEnemySettings.footClips.Count);

                if (animationEvent.intParameter == 0)
                    currentVolume = 0.2f;
                else if (animationEvent.intParameter == 1)
                    currentVolume = 0.3f;
                else if (animationEvent.intParameter == 2)
                    currentVolume = 0.4f;
                else
                    currentVolume = 0.1f;

                AudioSource.PlayClipAtPoint(healthEnemyResponse.statsEnemySettings.footClips[index], transform.TransformPoint(new Vector3(0.0f, transform.position.y, 0.0f)), currentVolume);
            }
        }
    }
}
