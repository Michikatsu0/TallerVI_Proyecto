using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base Data Enemy", order = 1)]
public class BaseEnemySettings : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] public float alertDistance, maxAlertDistance , searchYlimit, lerpSearchPosTarget, timeToStartAlert, timeToEndAlert, timeToSearchPos;

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
