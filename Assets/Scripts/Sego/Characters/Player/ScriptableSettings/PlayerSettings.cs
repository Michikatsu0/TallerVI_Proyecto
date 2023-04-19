using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base Mechanics Data Player", order = 1)]
public class PlayerSettings : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Joystick Settings")]
    [SerializeField][Range(0f, 1f)] public float deathZoneX;
    [SerializeField][Range(0f, 1f)] public float deathZoneAimXY;
    [SerializeField][Range(0f, 1f)] public float deathZoneJumpY;
    [SerializeField][Range(0f, 1f)] public float deathZoneCrouchY;

    [Header("Movement Settings")]
    [SerializeField][Range(0f, 100f)] public float movementSpeedMultiplier;
    [SerializeField][Range(0f, 1f)] public float movementSpeed;
    [SerializeField][Range(0f, 2f)] public float xMoveSpeed;

    [Header("Rotation Settings")]
    [SerializeField][Range(0f, 0.2f)] public float turnSmoothTime;

    [Header("Crouch Movement Settings")]
    [SerializeField][Range(0f, 100f)] public float crouchSpeedMultiplier;
    [SerializeField] public float crouchSpeed, crouchCenter;
    [SerializeField][Range(0f, 1f)] public float topHitDistance, crouchTopHitRadiusDistance;

    [Header("Gravity Settings")]
    [SerializeField][Range(0f, 100f)] public float gravityMultiplierPercentage;
    [SerializeField] public float gravityMultiplier;
    [SerializeField][Range(-1, -20)] public float groundGravity;

    [Header("Jump Settings")]
    [SerializeField] public float maxNumberOfJumps;
    [SerializeField][Range(0f, 100f)] public float jumpForceMultiplier;
    [SerializeField] public float jumpForce;
    [SerializeField][Range(0f, 100f)] public float jumpSpeedMultiplier;
    [SerializeField] public float jumpSpeed;

    [Header("Fall Settings")]
    [SerializeField][Range(0f, 1f)] public float centerDistance;
    [SerializeField][Range(0f, 15f)] public float movementAnimSpeed;
    [SerializeField][Range(0f, 5f)] public float heavyFallMoveDuration;
    [SerializeField] public LayerMask isGround;

    [Header("Dash Settings")]
    [SerializeField][Range(0f, 1f)] public float dashDuration;
    [SerializeField][Range(0f, 10f)] public float dashCoolDown;
    [SerializeField][Range(0f, 100f)] public float dashForceMultiplier;
    [SerializeField] public float dashForce;

    [Header("Slopes Settings")]
    [SerializeField][Range(0f, 1f)] public float slopeRayDistance;
    [SerializeField] public float slideSlopeSpeed, slopeforceDown;
    [SerializeField][Range(0f, 1f)] public float slopeRadiusDistance;

    [Header("Push RGBD's Settings")]
    [SerializeField][Range(0f, 100f)] public float pushPowerBridgesMultiplier;
    [SerializeField] public float pushPowerBridges, pushDelay;
    [SerializeField][Range(0f, 100f)] public float pushPowerProbsMultiplier;
    [SerializeField] public float pushPowerProbs;

    [Header("Aim Speed Movement Settings")]
    [SerializeField][Range(0f, 0.2f)] public float turnAimSmoothTime;
    [SerializeField][Range(0f, 100f)] public float aimSpeedMultiplier;
    [SerializeField] public float aimSpeed;

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
