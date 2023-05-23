using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base Data Enemy Stats", order = 1)]
public class StatsEnemySettings : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Health Settings")]
    [SerializeField] public float maxHealth;
    [SerializeField] public float deathTime;
    [Header("Blink Settings")]
    [SerializeField] public float blinkIntensity;
    [SerializeField] public float blinkDuration;

    [Header("Hit Effect Settings")]
    [SerializeField] public List<Color> sliderColors;
    [SerializeField] public float transitionDamageLerp;
    [SerializeField] public Material turretMaterial;

    [Header("Audio Settings")]
    [SerializeField] public List<AudioClip> deathClips;

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
