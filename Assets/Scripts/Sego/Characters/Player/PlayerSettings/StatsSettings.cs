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
    [SerializeField] public float maxTimeInvincible, deathTime, timeToRegenerate, regenerationSpeed;
    
    [Header("Health Slider Settings")]
    [SerializeField] public List<Color> sliderColors;
    [SerializeField] public float transitionDamageLerp;

    [Header("Hit Effect Settings")]
    [SerializeField] public float blinkIntensity, blinkDuration;
    [SerializeField] public List<Color> armatureColorsMaterial;
    [SerializeField] public List<Material> armatureHelmetMaterials;

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

