using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMechanicProvider1
{
    void Movement(float runSpeed, float runSpeedPercentage, float jumpSpeedMultiplier ,float deathZoneX, float deathZoneY, Joystick joystick);
    void Rotation(float turnSmoothTime, float deathZoneX, Joystick joystick);
    void Gravity(float gravityMultiplier, float gravityMultiplierPercent, float groundedGravity);
    void Jump(float jumpForce, float jumpForcePercentage, float deathZoneY, Joystick joystick);
    void PushObjects(float forceBridges, float forceBridgesMultiplier, float forceBridgesPercentage, float forceProbs, float forceProbsPercentage);
}
