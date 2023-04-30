using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponTypes
{
    SA,
    FA,
    BM,
}

public class WeaponSettings : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] public WeaponTypes weaponType;
    [SerializeField] public Color colorMuzzle;


    [SerializeField] public float fireRate;
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
