using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Base Data Turret Gun Laser", order = 1)]
public class TurretGunLaserSettings : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Aim Settings")]
    [SerializeField] public float lerpAimWeight;

    [Header("Render Settings")]
    [SerializeField] public Material emissionColor;
    [SerializeField] public List<Color> turrentColors;
    public void Init()
    {

    }
    public void OnAfterDeserialize()
    {
        Init();
    }

    public void OnBeforeSerialize()
    {

    }

}
