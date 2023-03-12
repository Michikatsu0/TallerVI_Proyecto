using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PositionPlatformsResponse : MonoBehaviour, IPositionPlatformsProvider
{
    public enum PlatformPositionTypes { Vertical, Horizontal, Both }

    [Header("Position Movement Platforms")]
    [SerializeField] private PlatformPositionTypes platformMoveType;
    [SerializeField] AnimationCurve positionCurveX;
    [SerializeField] AnimationCurve positionCurveY;
    [SerializeField] private float amplitude, speedMultiplier = 1;
    [SerializeField][Range(0f, 100f)] private float movementSpeedPercentage;

    private float time, duration, vertical, horizontal, percent;
    private Vector2 position;

    private void Awake()
    {
        time = 0;
        duration = 1;
        vertical = transform.position.y;
        horizontal = transform.position.x;
    }

    public void PositionPlatform()
    {
        percent = (speedMultiplier * movementSpeedPercentage) / 100;
        position = transform.position;
        if (PlatformPositionTypes.Vertical == platformMoveType) // Vertical movement
            position.y = amplitude * positionCurveY.Evaluate(percent * time) + vertical; 
        else if (PlatformPositionTypes.Horizontal == platformMoveType) // Horizontal movement
            position.x = amplitude * positionCurveX.Evaluate(percent * time) + horizontal;
        else if (PlatformPositionTypes.Both == platformMoveType) // Diagonal movement
        {
            position.x = amplitude * positionCurveX.Evaluate(percent * time) + horizontal;
            position.y = amplitude * positionCurveY.Evaluate(percent * time) + vertical;
        }
        transform.position = position;
        time += Time.deltaTime;

        if (time >= duration / percent)
            time = 0;
    }

   
   
}
