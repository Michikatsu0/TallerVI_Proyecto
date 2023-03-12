using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMechanicResponse1 : MonoBehaviour, IPlayerMechanicProvider
{
    //¡¡¡ScriptableObject Implementation for multiples intance in behaviour!!!//

    //movement variables
    private CharacterController characterController;
    private Vector3 zAxisMovement;
    private Vector2 currentDirection;
    private Vector2 appliedMovement;
    private float runPercent, zAxisMoveDirection = 1;
    //rotation variables
    private float defaultRotation = 90, currentRotation, turnSmoothVelocity;
    //gravity variables
    private float gravity = Physics.gravity.y, gravityPercent;
    //jump variables 
    private float jumpPercent;
    private bool isJumping = false;
    //slopes variables
    private float slopeSpeedPercent, groundRayDistance = 1;
    private RaycastHit slopeHit;
    private Vector3 offsetRay;
    //move rgbd's variables
    private enum ObjectTypes { Bridges, Probs }
    private ObjectTypes objectType;
    private float forcePercent;
    private Vector3 forceDirection;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        transform.rotation = Quaternion.Euler(0f, defaultRotation, 0f);
        currentRotation = defaultRotation;
    }
    private bool IsGrounded() => characterController.isGrounded;
    public void Jump(float jumpForce, float jumpForcePercentage, float deathZoneY, Joystick joystick)
    {
        jumpPercent = (jumpForce * jumpForcePercentage) / 100;

        if (IsGrounded() && !isJumping && joystick.Vertical >= deathZoneY) 
        {
            isJumping = true;
            currentDirection.y = jumpForce * jumpPercent;
            appliedMovement.y = jumpForce * jumpPercent;
        }
        else if (isJumping && IsGrounded() && joystick.Vertical < deathZoneY)
            isJumping = false;
    }

    public void Gravity(float gravityMultiplier, float gravityMultiplierPercent, float groundedGravity)
    {
        gravityPercent = (gravityMultiplier * gravityMultiplierPercent) / 100;

        if (IsGrounded() && currentDirection.y < 0.0f)
        {
            currentDirection.y = groundedGravity;
            appliedMovement.y = groundedGravity;
        } 
        else 
        {
            float previousYVelocity = currentDirection.y;
            currentDirection.y = currentDirection.y + (gravity * gravityPercent *Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentDirection.y) * 0.5f, -20.0f);
        }
    }

    public void PushObjects(float forceBridges, float forceBridgesMultiplier, float forceBridgesPercentage, float forceProbs, float forceProbsPercentage) 
    {
        switch (objectType)
        {
            case ObjectTypes.Bridges:
                forcePercent = ((forceBridges * forceBridgesMultiplier) * forceBridgesPercentage) / 100;
                break;
            case ObjectTypes.Probs:
                forcePercent = (forceProbs * forceProbsPercentage) / 100;
                break;
        }
    }

    public void Rotation(float turnSmoothTime, float deathZoneX, Joystick joystick)
    {
        if (joystick.Horizontal > deathZoneX / 2) currentRotation = defaultRotation;
        else if (joystick.Horizontal < -deathZoneX / 2) currentRotation = 3 * defaultRotation;

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, currentRotation, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public void Movement(float runSpeed,float runSpeedPercentage, float jumpSpeedMultiplier, float deathZoneX, float deathZoneY, Joystick joystick)
    {
        if (transform.position.z < -0.001f)
        {
            zAxisMovement.z = zAxisMoveDirection;
            characterController.Move(zAxisMovement * Time.deltaTime);
        }
        else if (transform.position.z > 0.001f)
        {
            zAxisMovement.z = -zAxisMoveDirection;
            characterController.Move(zAxisMovement * Time.deltaTime);
        }

        if (!IsGrounded() && isJumping) runPercent = ((runSpeed + jumpSpeedMultiplier) * runSpeedPercentage) / 100;
        else runPercent = (runSpeed * runSpeedPercentage) / 100;
        
        currentDirection.x = joystick.Horizontal;
        if (joystick.Horizontal >= deathZoneX) appliedMovement.x = currentDirection.x * runPercent;
        else if (joystick.Horizontal <= -deathZoneX) appliedMovement.x = currentDirection.x * runPercent;
        else appliedMovement.x = 0;

        characterController.Move(appliedMovement * Time.deltaTime);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rgbd = hit.collider.attachedRigidbody;
        if (rgbd == null || rgbd.isKinematic) return;

        GameObject gameObject = hit.gameObject;
        if (gameObject.CompareTag("Bridges"))
        {
            objectType = ObjectTypes.Bridges;
            forceDirection.x = hit.moveDirection.x;
            forceDirection.y = hit.moveDirection.y;
            rgbd.velocity = forceDirection * forcePercent / rgbd.mass;
        }
        else if (gameObject.CompareTag("Probs"))
        {
            if (hit.moveDirection.y < -0.1f) return;
            objectType = ObjectTypes.Probs;
            forceDirection = hit.gameObject.transform.position - transform.position;
            forceDirection.y = 0;
            rgbd.AddForceAtPosition(forceDirection * forcePercent, transform.position, ForceMode.Impulse);
        }
    }
}
