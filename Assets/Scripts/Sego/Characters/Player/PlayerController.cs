using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMechanicResponse))]
public class PlayerController : MonoBehaviour
{
    [Header("Joystick Settings")]
    [SerializeField] public Joystick rightJoystick;
    [SerializeField] public Joystick leftJoystick;
    [SerializeField] [Range(0f, 1f)] public float deathZoneX;
    [SerializeField] [Range(0f, 1f)] public float deathZoneAimXY;
    [SerializeField] [Range(0f, 1f)] public float deathZoneJumpY;
    [SerializeField] [Range(0f, 1f)] public float deathZoneCrouchY;

    [Header("Gravity Settings")]
    [SerializeField] [Range(0f, 100f)] private float gravityMultiplierPercentage;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] [Range(-1,-20)] private float groundGravity;
     
    [Header("Fall Settings")]
    [SerializeField][Range(0f, 1f)] private float centerDistance;
    [SerializeField] private LayerMask isGround;

    [Header("Slopes Settings")]
    [SerializeField] [Range(0f, 1f)] private float slopeRayDistance;
    [SerializeField] private float slideSlopeSpeed, slopeforceDown;


    [Header("Slopes Settings")]
    [SerializeField][Range(0f, 0.2f)] private float turnAimSmoothTime;
    [SerializeField][Range(0f, 100f)] private float aimSpeedMultiplier;
    [SerializeField] private float aimSpeed;

    [Header("Movement Settings")]
    [SerializeField] [Range(0f, 100f)] private float movementSpeedMultiplier;
    [SerializeField] private float movementSpeed;

    [Header("Crouch Movement Settings")]
    [SerializeField] [Range(0f, 100f)] private float crouchSpeedMultiplier;
    [SerializeField] private float crouchSpeed;

    [Header("Rotation Settings")]
    [SerializeField] [Range(0f, 0.2f)] private float turnSmoothTime;

    [Header("Jump Settings")]
    [SerializeField] private float maxNumberOfJumps;
    [SerializeField] [Range(0f, 100f)] private float jumpForceMultiplier;
    [SerializeField] private float jumpForce;
    [SerializeField] [Range(0f, 100f)] private float jumpSpeedMultiplier;
    [SerializeField] private float jumpSpeed;

    [Header("Push RGBD's Settings")]
    [SerializeField][Range(0f, 100f)] private float pushPowerBridgesMultiplier;
    [SerializeField] private float pushPowerBridges, pushDelay;
    [SerializeField][Range(0f, 100f)] private float pushPowerProbsMultiplier;
    [SerializeField] private float pushPowerProbs;

    private IPlayerMechanicProvider playerMechanicsProvider;

    private void Awake()
    {
        playerMechanicsProvider = GetComponent<IPlayerMechanicProvider>();

        playerMechanicsProvider.StartInputs(deathZoneX, deathZoneJumpY, deathZoneCrouchY, deathZoneAimXY, rightJoystick, leftJoystick);
    }

    void Update()
    {
        playerMechanicsProvider.Gravity(gravityMultiplier, gravityMultiplierPercentage, groundGravity);
        playerMechanicsProvider.SlopeSlide(slopeRayDistance, slideSlopeSpeed, slopeforceDown);
        playerMechanicsProvider.PushObjects(pushPowerBridges, pushPowerBridgesMultiplier, pushDelay, pushPowerProbs, pushPowerProbsMultiplier);
        playerMechanicsProvider.Fall(centerDistance, isGround);
        playerMechanicsProvider.Jump(maxNumberOfJumps, jumpForce, jumpForceMultiplier, jumpSpeed, jumpSpeedMultiplier);
        playerMechanicsProvider.Crouch(crouchSpeed, crouchSpeedMultiplier);     
        playerMechanicsProvider.Rotation(turnSmoothTime);
        playerMechanicsProvider.Aim(turnAimSmoothTime, aimSpeed, aimSpeedMultiplier);
        playerMechanicsProvider.Movement(movementSpeed, movementSpeedMultiplier);

    }

}
