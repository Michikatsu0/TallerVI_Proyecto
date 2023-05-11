using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponTypes
{
    SA,
    FA,
    BM,
}

[CreateAssetMenu(menuName = "Base Data Weapon", order = 1)]
public class WeaponSettings : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] public WeaponTypes weaponType;
    [SerializeField] public Color colorMuzzle;


    [SerializeField] public float fireRate = 25, bulletSpeed = 1000, bulletDrop, maxLifeTime;
    [SerializeField] public bool isFiring;
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
