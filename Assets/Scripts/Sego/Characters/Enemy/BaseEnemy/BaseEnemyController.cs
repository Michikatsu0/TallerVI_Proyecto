using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseEnemyController : MonoBehaviour
{
    [SerializeField] protected BaseEnemySettings baseEnemySettings;

    [SerializeField] protected Transform playerTarget;
    protected float startAlertTime, currentDistance, currentMaxDistance;
    public bool onAlert;

    public bool OnAlert { get => onAlert; set => onAlert = value; }

    protected void TimingDistanceAlert()
    {
        if (onAlert) return;
        currentDistance = Vector3.Distance(transform.position, playerTarget.position);
        if (currentDistance <= baseEnemySettings.alertDistance)
        {
            startAlertTime += Time.deltaTime;
            if (startAlertTime >= baseEnemySettings.timeToStartAlert)
            {
                onAlert = true;
            }
        }
        else
        {
            startAlertTime = 0;
        }

        currentMaxDistance = Vector3.Distance(transform.position, playerTarget.position);


        if (currentMaxDistance >= baseEnemySettings.maxAlertDistance)
        {
            
        }
    }

}

