using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CircularPlatformsResponse : MonoBehaviour, ICircularPlatformProvider
{
    [Header("Cicular Movement Platforms")]
    [SerializeField] private Transform centerPoint;
    [SerializeField][Range(0f, 100f)] private float angularSpeedPercentage;
    [SerializeField] private float radiusX, radiusY, angularSpeedMultiplier;

    private float percent, angle;
    private Vector2 position;

    private void LateUpdate()
    {
        CircularPlatform();
    }

    public void CircularPlatform()
    {
        percent = (angularSpeedMultiplier * angularSpeedPercentage) / 360;
        position.x = centerPoint.position.x + Mathf.Cos(percent * angle) * radiusX;
        position.y = centerPoint.position.y + Mathf.Sin(percent * angle) * radiusY;

        transform.position = position;
        angle += Time.deltaTime;
        if (angle >= 360)
            angle = 0;
    }
}
