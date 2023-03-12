using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    [Header("Joystick Settings")]
    [SerializeField] private Joystick joystick;
    [SerializeField] [Range(0f, 1f)] private float deathZoneX;
    [SerializeField] [Range(0f, 1f)] private float deathZoneY;

    [Header("Gravity Settings")]
    [SerializeField] private float gravityMultiplier;
    [SerializeField] [Range(0f, 100f)] private float gravityMultiplierPercentage;
    [SerializeField] private float groundedGravity = -4.5f;

    [Header("Player Movement Settings")]
    [SerializeField] private float runSpeed;
    [SerializeField] [Range(0f, 100f)] private float runSpeedPercentage;

    [Header("Player Push Objects Settings")]
    [SerializeField] private float forceBridges;
    [SerializeField] [Range(1f, 5f)] private float forceBridgesMultiplier;
    [SerializeField] [Range(0f, 100f)] private float forceBridgesPercentage;
    [SerializeField] private float forceProbs;
    [SerializeField][Range(0f, 100f)] private float forceProbsPercentage;

    [Header("Player Slopes Movement Settings")]
    [SerializeField] private float slopeSlideSpeed;
    [SerializeField] [Range(0f, 100f)] private float slopeSlideSpeedPercentage;

    [Header("Player Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] [Range(0f, 100f)] private float jumpForcePercentage;
    [SerializeField] [Range(1f, 5f)] private float jumpSpeedMultiplier;

    [Header("Player Rotation Settings")]
    [SerializeField][Range(0f, 1f)] private float turnSmoothTime;

    private IPlayerMechanicProvider playerMechanicsProvider;

    void Start()
    {
        playerMechanicsProvider = GetComponent<IPlayerMechanicProvider>();
    }

    void Update()
    {
        playerMechanicsProvider.Gravity(gravityMultiplier, gravityMultiplierPercentage, groundedGravity); 
        playerMechanicsProvider.Rotation(turnSmoothTime, deathZoneX, joystick);
        playerMechanicsProvider.Jump(jumpForce, jumpForcePercentage, deathZoneY, joystick);
        playerMechanicsProvider.Movement(runSpeed, runSpeedPercentage, jumpSpeedMultiplier ,deathZoneX, deathZoneY, joystick);
        playerMechanicsProvider.PushObjects(forceBridges, forceBridgesMultiplier, forceBridgesPercentage, forceProbs, forceProbsPercentage);
    }
}
