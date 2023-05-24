using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Base Coin Audio Data", order = 1)]

public class ScifiCoinSettings : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Audio Settings")]
    [SerializeField] public List<AudioClip> coinClips;
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