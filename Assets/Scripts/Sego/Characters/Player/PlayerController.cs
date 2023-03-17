using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMechanicResponse))]
public class PlayerController : MonoBehaviour
{
    [Header("Joystick Settings")]
    [SerializeField] public Joystick joystick;
    [SerializeField] [Range(0f, 1f)] public float deathZoneX;
    [SerializeField] [Range(0f, 1f)] public float deathZoneJumpY;
    [SerializeField] [Range(0f, 1f)] public float deathZoneCrouchY;

    [Header("Gravity Settings")]
    [SerializeField] [Range(0f, 100f)] private float gravityMultiplierPercentage;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] [Range(-1,-20)] private float groundGravity;

    [Header("Fall Settings")]
    [SerializeField] private float distance;
    [SerializeField] private LayerMask isGround;


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

    private IPlayerMechanicProvider playerMechanicsProvider;

    private void Awake()
    {
        playerMechanicsProvider = GetComponent<IPlayerMechanicProvider>();
        playerMechanicsProvider.StartInputs(deathZoneX, deathZoneJumpY, deathZoneCrouchY, joystick);
    }

    void Update()
    {
        playerMechanicsProvider.Gravity(gravityMultiplier, gravityMultiplierPercentage, groundGravity);
        playerMechanicsProvider.Fall(distance, isGround);
        playerMechanicsProvider.Crouch(crouchSpeed, crouchSpeedMultiplier);
        playerMechanicsProvider.Jump(maxNumberOfJumps, jumpForce, jumpForceMultiplier, jumpSpeed, jumpSpeedMultiplier);
        playerMechanicsProvider.Rotation(turnSmoothTime);
        playerMechanicsProvider.Movement(movementSpeed, movementSpeedMultiplier);
    }



}
