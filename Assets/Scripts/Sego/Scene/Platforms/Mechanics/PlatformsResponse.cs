using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsResponse : MonoBehaviour , IPlatformsProvider
{
    [Header("Position Movement Platforms")]
    [SerializeField] AnimationCurve positionCurveX;
    [SerializeField] AnimationCurve positionCurveY;
    [SerializeField] private float amplitude, speedMultiplier = 1;
    [SerializeField] [Range(0f, 100f)] private float movementSpeedPercentage;
    private Vector2 position;
    private float time, duration, vertical, horizontal, percent;

    [Header("Rotation Slope Platforms")]
    [SerializeField] AnimationCurve rotationCurveEven;
    [SerializeField] AnimationCurve rotationCurveNotEven;
    [SerializeField] List<GameObject> platforms = new List<GameObject>();
    [SerializeField] private float rotationSlope, rotationSpeedMultiplier = 1, angleOffset;
    [SerializeField] [Range(0f, 100f)] private float rotationSpeedPercentage;
    private Vector3 rotation;


    [Header("Cicular Movement Platforms")]
    [SerializeField] private Transform centerPoint;
    [SerializeField] [Range(0f, 100f)] private float angularSpeedPercentage;
    [SerializeField] private float radiusX, radiusY, angularSpeedMultiplier;
    private float angle;

    private void Awake()
    {
        time = 0;
        duration = 1;
        vertical = transform.position.y;
        horizontal = transform.position.x;
    }

    public void PositionPlatform(PlatformMoveTypes platformType)
    {
        percent = (speedMultiplier * movementSpeedPercentage) / 100;
        position = transform.position;
        if (PlatformMoveTypes.Vertical == platformType) // Vertical movement
            position.y = amplitude * positionCurveY.Evaluate(percent * time) + vertical; 
        else if (PlatformMoveTypes.Horizontal == platformType) // Horizontal movement
            position.x = amplitude * positionCurveX.Evaluate(percent * time) + horizontal;
        else if (PlatformMoveTypes.Both == platformType) // Diagonal movement
        {
            position.x = amplitude * positionCurveX.Evaluate(percent * time) + horizontal;
            position.y = amplitude * positionCurveY.Evaluate(percent * time) + vertical;
        }
        transform.position = position;
        time += Time.deltaTime;

        if (time >= duration / percent)
            time = 0;
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
