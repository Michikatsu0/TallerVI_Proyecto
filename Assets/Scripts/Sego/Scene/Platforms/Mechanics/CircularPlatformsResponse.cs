using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class CircularPlatformsResponse : MonoBehaviour, ICircularPlatformProvider
{
    [Header("Cicular Movement Platforms")]
    [SerializeField] private Transform centerPoint;
    [SerializeField] private float radiusX, radiusY, angularSpeedMultiplier;
    [SerializeField][Range(0f, 100f)] private float angularSpeedPercentage;

    private float percent, angle;
    private Vector2 position;
    private Rigidbody rgbd;
    private void Awake()
    {
        rgbd = GetComponent<Rigidbody>();
    }
    public void CircularPlatform()
    {
        percent = (angularSpeedMultiplier * angularSpeedPercentage) / 360;
        position.x = centerPoint.position.x + Mathf.Cos(percent * angle) * radiusX;
        position.y = centerPoint.position.y + Mathf.Sin(percent * angle) * radiusY;

        rgbd.MovePosition(position);
        angle += Time.deltaTime;
        if (angle >= 360)
            angle = 0;
    }
}
