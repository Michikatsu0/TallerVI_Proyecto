using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseEnemyController : MonoBehaviour
{
    [SerializeField] protected BaseEnemySettings baseEnemySettings;

    protected Transform playerTarget;
    protected float startTime, currentDistance;
    public bool onAlert;

    public bool OnAlert { get => onAlert; set => onAlert = value; }

    protected void TimingDistanceAlert()
    {
        if (onAlert) return;
        currentDistance = Vector3.Distance(transform.position, playerTarget.position);
        if (currentDistance <= baseEnemySettings.alertDistance)
        {
            startTime += Time.deltaTime;
            if (startTime >= baseEnemySettings.timeToStartAlert)
            {
                onAlert = true;
            }
        }
        else
        {
            startTime = 0;
        }
    }

}

