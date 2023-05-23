using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseEnemyController : MonoBehaviour
{
    [SerializeField] protected BaseEnemySettings baseEnemySettings;
    
    protected Transform playerTarget;
    protected float startAlertTime, endAlertTime, currentDistance, currentMaxDistance;

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
                endAlertTime = baseEnemySettings.timeToEndAlert;
            }
        }
        else
        {
            startAlertTime = 0;
        }

        currentMaxDistance = Vector3.Distance(transform.position, playerTarget.position);


        if (currentMaxDistance >= baseEnemySettings.maxAlertDistance)
        {
            endAlertTime -= Time.deltaTime;
            if (endAlertTime <= 0)
            {
                onAlert = false;
            }
        }
    }

   
}

