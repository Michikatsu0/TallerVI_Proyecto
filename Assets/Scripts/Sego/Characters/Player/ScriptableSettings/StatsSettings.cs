using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Base Stats Data Player", order = 1)]
public class StatsSettings : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Health Settings")]
    [SerializeField] public int maxHealth;
    [SerializeField] public float maxTimeInvincible, deathTime;



    public void Init()
    {

    }
    public void OnBeforeSerialize()
    {
        Init();
    }

    public void OnAfterDeserialize()
    {

    }
}

