using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMechanicResponse))]
public class PlayerController : MonoBehaviour
{
    [Header("Joystick Settings")]
    [SerializeField] private Joystick rightJoystick;
    [SerializeField] private Joystick leftJoystick;
    
    [Header("Player Settings")]
    [SerializeField] private PlayerSettings playerSettings;
    private IPlayerMechanicProvider playerMechanicsProvider;

    private void Awake()
    {
        playerMechanicsProvider = GetComponent<IPlayerMechanicProvider>();

        playerMechanicsProvider.StartInputs(playerSettings.deathZoneX, playerSettings.deathZoneJumpY, playerSettings.deathZoneCrouchY, playerSettings.deathZoneAimXY, rightJoystick, leftJoystick);
    }

    void Update()
    {
        playerMechanicsProvider.Gravity(playerSettings.gravityMultiplier, playerSettings.gravityMultiplierPercentage, playerSettings.groundGravity);
        playerMechanicsProvider.SlopeSlide(playerSettings.slopeRayDistance, playerSettings.slideSlopeSpeed, playerSettings.slopeforceDown);
        playerMechanicsProvider.PushObjects(playerSettings.pushPowerBridges, playerSettings.pushPowerBridgesMultiplier, playerSettings.pushDelay, playerSettings.pushPowerProbs, playerSettings.pushPowerProbsMultiplier);
        playerMechanicsProvider.Fall(playerSettings.centerDistance, playerSettings.isGround);
        playerMechanicsProvider.Jump(playerSettings.maxNumberOfJumps, playerSettings.jumpForce, playerSettings.jumpForceMultiplier, playerSettings.jumpSpeed, playerSettings.jumpSpeedMultiplier);
        playerMechanicsProvider.Crouch(playerSettings.crouchSpeed, playerSettings.crouchSpeedMultiplier);     
        playerMechanicsProvider.Rotation(playerSettings.turnSmoothTime);
        playerMechanicsProvider.Aim(playerSettings.turnAimSmoothTime, playerSettings.aimSpeed, playerSettings.aimSpeedMultiplier);
        playerMechanicsProvider.Movement(playerSettings.movementSpeed, playerSettings.movementSpeedMultiplier);

    }

}
