using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CircularPlatformsResponse : MonoBehaviour, ICircularPlatformProvider
{
    [Header("Cicular Movement Platforms")]
    [SerializeField] private Transform centerPoint;
    [SerializeField] private float radiusZ, radiusY, angularSpeedMultiplier;
    [SerializeField][Range(0f, 100f)] private float angularSpeedPercentage;

    private float percent, angle;
    private Vector3 position;

    public void CircularPlatform()
    {
        percent = (angularSpeedMultiplier * angularSpeedPercentage) / 360;
        position.z = centerPoint.position.z + Mathf.Cos(percent * angle) * radiusZ;
        position.y = centerPoint.position.y + Mathf.Sin(percent * angle) * radiusY;

        transform.position = position;
        angle += Time.deltaTime;
        if (angle >= 360)
            angle = 0;
    }
}
