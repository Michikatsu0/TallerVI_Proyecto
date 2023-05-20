using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base Data Weapon", order = 1)]
public class WeaponSettings : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] public TrailRenderer tracerEffect;

    [SerializeField] public Color colorMuzzle;

    [SerializeField] public int maxNumberBounces;

    [SerializeField] public int damage;

    [SerializeField] public float fireRate = 25, bulletSpeed = 1000, bulletDrop, maxLifeTime;

    [SerializeField] public bool isFiring;

    [SerializeField] public List<AudioClip> weaponHitsAudioClips;
    [SerializeField] public List<AudioClip> weaponShootAudioClips;
    [SerializeField] public AudioClip reloadAudioClip;

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
