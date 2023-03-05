using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsResponse : MonoBehaviour , IPlatformsProvider
{
    [Header("Position Movement Platforms")]
    [SerializeField] AnimationCurve positionCurve;
    [SerializeField] private float amplitude, speedMultiplier = 1;
    [Header("Speed in Percentage [0%, 100%]")]
    [SerializeField] [Range(0f, 100f)] private float speedPercentage;
    private Vector2 position;
    float time, duration, vertical, horizontal, percent, angleOffset;

    [Header("Rotation Movement Platforms")]
    [SerializeField] AnimationCurve rotationCurve;
    [SerializeField] private float rotationSlope ,rotationSpeedMultiplier = 1;
    [SerializeField] List<GameObject> gameObjects = new List<GameObject>();
    [SerializeField] [Range(0f, 100f)] private float rotationSpeedPercentage;
    private Vector3 rotation;
    private void Awake()
    {
        time = 0;
        duration = 1;
        vertical = transform.position.y;
        horizontal = transform.position.x;
    }

    public void PositionPlatform(PlatformMoveTypes platformType,Vector3 position)
    {
        percent = (speedMultiplier * speedPercentage) / 100;
        this.position = position;
        if (PlatformMoveTypes.Vertical == platformType) // Vertical movement
            this.position.y = amplitude * positionCurve.Evaluate(percent * time) + vertical; 
        else if (PlatformMoveTypes.Horizontal == platformType) // Horizontal movement
            this.position.x = amplitude * positionCurve.Evaluate(percent * time) + horizontal;
        else if (PlatformMoveTypes.Both == platformType) // Diagonal movement
        {
            this.position.x = amplitude * positionCurve.Evaluate(percent * time) + horizontal;
            this.position.y = amplitude * positionCurve.Evaluate(percent * time) + vertical;
        }
        transform.position = this.position;
        time += Time.deltaTime;

        if (time >= duration / percent)
            time = 0;
    }

    public void RotationPlatform(Vector3 rotation)
    {
        percent = (rotationSpeedMultiplier * rotationSpeedPercentage) / 100;
        this.rotation = rotation;
        this.rotation.z = amplitude * rotationCurve.Evaluate(percent * time) + angleOffset;
        transform.localEulerAngles = this.rotation;
        time += Time.deltaTime;
        if (time >= duration / percent)
            time = 0;
    }

    public void CircularPlatform(Vector3 position)
    {

    }
}
