using System;
using System.Collections.Generic;
using UnityEngine;

public class RotationPlatformsResponse : MonoBehaviour, IRotationPlatformProvider
{
    [Header("Rotation Slope Platforms")]
    [SerializeField] AnimationCurve rotationCurveEven;
    [SerializeField] AnimationCurve rotationCurveNotEven;
    [SerializeField] List<GameObject> platforms = new List<GameObject>();
    [SerializeField] private float rotationSlope, rotationSpeedMultiplier = 1, angleOffset;
    [SerializeField][Range(0f, 100f)] private float rotationSpeedPercentage;
    
    private float time, duration, percent;
    private Vector3 rotation;
    private void Awake()
    {
        time = 0;
        duration = 1;
        transform.rotation = Quaternion.Euler(0f,0f,rotationSlope);
    }
    private void LateUpdate()
    {
        RotationPlatform();
    }
    public void RotationPlatform()
    {

        percent = (rotationSpeedMultiplier * rotationSpeedPercentage) / 100;

        for (int i = 0; i < platforms.Count; i++)
        {
            if (i % 2 == 0)
            {
                rotation = platforms[i].transform.rotation.eulerAngles;
                rotation.z = rotationSlope * rotationCurveEven.Evaluate(percent * time) + angleOffset;
                platforms[i].transform.rotation = Quaternion.Euler(0, 0, rotation.z);
            }
            else
            {
                rotation = platforms[i].transform.rotation.eulerAngles;
                rotation.z = rotationSlope * rotationCurveNotEven.Evaluate(percent * time) + angleOffset;
                platforms[i].transform.rotation = Quaternion.Euler(0, 0, rotation.z);
            }
        }
        time += Time.deltaTime;
        if (time >= duration / percent)
            time = 0;
    }

}
