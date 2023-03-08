using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMechanicProvider
{
    void Movement(float runSpeed, float deathZone, float turnSmoothTime, Vector3 direction, Joystick joystick);
    void Rotation(float turnSmoothTime, Joystick joystick);
    void JumpAndGravity();
}
