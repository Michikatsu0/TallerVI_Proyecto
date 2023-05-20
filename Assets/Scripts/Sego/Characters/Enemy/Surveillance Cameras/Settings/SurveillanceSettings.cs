using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base Data Surveillance Cam", order = 1)]
public class SurveillanceSettings : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Alert State Settings")]
    [SerializeField] public AlertSystemStates alertSystemState;

    [Header("Alert Sign Settings")]
    [SerializeField] public Vector3 center;
    [SerializeField] public LayerMask isEnemy;
    [SerializeField] public float radiusSignRange;

    [Header("Alert Timing & Distances Settings")]
    [SerializeField] public float alertDistance;
    [SerializeField] public float timeToSearchPos, timeToStartAlert, timeToEndAlert, lerpAimWeight, searchYlimit, lerpSearchPosTarget;

    [Header("Alert Cam Render & UI Settings")]
    [SerializeField] public Material glassCam;
    [SerializeField] public float lerpTransitionColor, lerpTransitionSlider;
    [SerializeField] public List<Color> sliderColors;

    [Header("Audio Settings")]
    [SerializeField] public List<AudioClip> audioClips;

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
