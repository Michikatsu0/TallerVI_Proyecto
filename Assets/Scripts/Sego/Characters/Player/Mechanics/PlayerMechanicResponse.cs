using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanicResponse : MonoBehaviour, IPlayerMechanicProvider
{
    
    private CharacterController characterController;
    private float turnSmoothVelocity;
    private float defaultRotation = 90, currentRotation;
    private bool rotation = false;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        transform.rotation = Quaternion.Euler(0f, defaultRotation, 0f);
        currentRotation = defaultRotation;
    }

    public void JumpAndGravity()
    {
        throw new System.NotImplementedException();
    }

    public void Movement(float runSpeed, float deathZone, float turnSmoothTime, Vector3 direction, Joystick joystick)
    {
        direction.x = joystick.Horizontal * runSpeed * Time.deltaTime;

        if (joystick.Horizontal >= deathZone)
        {
            characterController.Move(direction);
        }
        else if (joystick.Horizontal <= -deathZone)
        {
            characterController.Move(direction);
        }
        Rotation(turnSmoothTime, joystick);
    }

    public void Rotation(float turnSmoothTime, Joystick joystick)
    {
        if (joystick.Horizontal > 0.15f)
            currentRotation = defaultRotation;
        else if (joystick.Horizontal < -0.15f)
        {
            if (rotation)
                currentRotation = 3 * defaultRotation;
            else
                currentRotation = - defaultRotation;
        }

        if (rotation) rotation= false;
        else rotation = true;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, currentRotation, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f,angle,0f);

        
    }

    
}
