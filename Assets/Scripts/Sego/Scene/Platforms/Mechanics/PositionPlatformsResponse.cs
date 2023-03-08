using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PositionPlatformsResponse : MonoBehaviour, IPositionPlatformsProvider
{
    public enum PlatformMoveTypes { Vertical, Horizontal, Both }

    [Header("Position Movement Platforms")]
    [SerializeField] private PlatformMoveTypes platformMoveType;
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
    private void LateUpdate()
    {
        PositionPlatform();
    }
    public void PositionPlatform()
    {
        percent = (speedMultiplier * movementSpeedPercentage) / 100;
        position = transform.position;
        if (PlatformMoveTypes.Vertical == platformMoveType) // Vertical movement
            position.y = amplitude * positionCurveY.Evaluate(percent * time) + vertical; 
        else if (PlatformMoveTypes.Horizontal == platformMoveType) // Horizontal movement
            position.x = amplitude * positionCurveX.Evaluate(percent * time) + horizontal;
        else if (PlatformMoveTypes.Both == platformMoveType) // Diagonal movement
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
