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
    [SerializeField] public float maxHealth;
    [SerializeField] public float maxHealthToRegenerate,deathTime, timeToRegenerate, regenerationSpeed;
    
    [Header("Health Slider Settings")]
    [SerializeField] public List<Color> sliderColors;
    [SerializeField] public float transitionDamageLerp;

    [Header("Hit Effect Settings")]
    [SerializeField] public float blinkIntensity;
    [SerializeField] public float blinkDuration;
    [SerializeField] public List<Color> armatureColorsMaterial;
    [SerializeField] public List<Material> armatureHelmetMaterials;
    
    [Header("Audio Settings")]
    [SerializeField] public float lerpAudioTransition;
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

