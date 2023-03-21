using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMechanicProvider
{
    void StartInputs(float deathZoneX, float deathZoneJumpY, float deathZoneCrouchY, Joystick joystick);
    void Jump(float maxNumberOfJumps, float jumpForce, float jumpForceMultiplier, float jumpSpeed, float jumpSpeedMultiplier);
    void Crouch(float crouchSpeed, float crouchSpeedMultiplier);
    void PushObjects(float pushPowerBridges, float pushPowerBridgesMultiplier, float pushDelay, float pushPowerProbs, float pushPowerProbsMultiplier);
    void Rotation(float turnSmoothTime);
    void Gravity(float gravityMultiplier, float gravityMultiplierPercent, float groundGravity);
    void SlopeSlide(float slopeRayDistance, float slideSlopeSpeed, float slopeforceDown);
    void Fall(float centerDistance, LayerMask isGround);
    void Movement(float movementSpeed, float movementSpeedMultiplier);
}
