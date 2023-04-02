using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base Data Player", order = 1)]
public class PlayerSettings : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Joystick Settings")]
    [SerializeField][Range(0f, 1f)] public float deathZoneX;
    [SerializeField][Range(0f, 1f)] public float deathZoneAimXY;
    [SerializeField][Range(0f, 1f)] public float deathZoneJumpY;
    [SerializeField][Range(0f, 1f)] public float deathZoneCrouchY;

    [Header("Gravity Settings")]
    [SerializeField][Range(0f, 100f)] public float gravityMultiplierPercentage;
    [SerializeField] public float gravityMultiplier;
    [SerializeField][Range(-1, -20)] public float groundGravity;

    [Header("Fall Settings")]
    [SerializeField][Range(0f, 1f)] public float centerDistance;
    [SerializeField] public LayerMask isGround;

    [Header("Slopes Settings")]
    [SerializeField][Range(0f, 1f)] public float slopeRayDistance;
    [SerializeField] public float slideSlopeSpeed, slopeforceDown;


    [Header("Slopes Settings")]
    [SerializeField][Range(0f, 0.2f)] public float turnAimSmoothTime;
    [SerializeField][Range(0f, 100f)] public float aimSpeedMultiplier;
    [SerializeField] public float aimSpeed;

    [Header("Movement Settings")]
    [SerializeField][Range(0f, 100f)] public float movementSpeedMultiplier;
    [SerializeField] public float movementSpeed;

    [Header("Crouch Movement Settings")]
    [SerializeField][Range(0f, 100f)] public float crouchSpeedMultiplier;
    [SerializeField] public float crouchSpeed;

    [Header("Health Settings")]
    [SerializeField] public int maxHealth;
    [SerializeField] public float maxTimeInvincible, deathTime;

    [Header("Rotation Settings")]
    [SerializeField][Range(0f, 0.2f)] public float turnSmoothTime;

    [Header("Jump Settings")]
    [SerializeField] public float maxNumberOfJumps;
    [SerializeField][Range(0f, 100f)] public float jumpForceMultiplier;
    [SerializeField] public float jumpForce;
    [SerializeField][Range(0f, 100f)] public float jumpSpeedMultiplier;
    [SerializeField] public float jumpSpeed;

    [Header("Push RGBD's Settings")]
    [SerializeField][Range(0f, 100f)] public float pushPowerBridgesMultiplier;
    [SerializeField] public float pushPowerBridges, pushDelay;
    [SerializeField][Range(0f, 100f)] public float pushPowerProbsMultiplier;
    [SerializeField] public float pushPowerProbs;

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
