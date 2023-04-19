using UnityEngine;

public interface IPlayerMechanicProvider
{
    void StartInputs(float deathZoneX, float deathZoneJumpY, float deathZoneCrouchY, float deathZoneAimXY, Joystick rightJoystick, Joystick leftJoystick);
    void Movement(float movementSpeed, float movementSpeedMultiplier, float xMoveSpeed);
    void Crouch(float crouchCenter, float crouchSpeed, float crouchSpeedMultiplier, float topHitDistance, float addRadiusDistance, LayerMask idGround);
    void Gravity(float gravityMultiplier, float gravityMultiplierPercent, float groundGravity);
    void Jump(float maxNumberOfJumps, float jumpForce, float jumpForceMultiplier, float jumpSpeed, float jumpSpeedMultiplier);
    void Fall(float centerDistance, float heavyFallMoveSpeed, float heavyFallMoveSpeedMultiplier, float heavyFallMoveDelay, float heavyFallMoveDuration, LayerMask isGround);
    void SlopeSlide(float slopeRayDistance, float slopeRadiusDistance, float slideSlopeSpeed, float slopeforceDown);
    void PushObjects(float pushPowerBridges, float pushPowerBridgesMultiplier, float pushDelay, float pushPowerProbs, float pushPowerProbsMultiplier);
    void Rotation(float turnSmoothTime);
    void Aim(float turnAimSmoothTime, float aimSpeed, float aimSpeedMultiplier);
    void Dash(float dashDuration, float dashCoolDown, float dashForce, float dashForceMultiplier);
}