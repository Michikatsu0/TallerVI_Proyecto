using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class PlayerMechanicResponse : MonoBehaviour, IPlayerMechanicProvider
{

    private enum PlayerStates { GroundMove, Jump, Crouch }
    private PlayerStates playerState;

    //general variables
    private CharacterController characterController;
    ////Slope variables
    //private RaycastHit slopeHit;
    //private Vector3 rayHelperPos;
    //private bool exitingSlope;
    //private float maxSlopeAngle, offset = 0.99f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        transform.rotation = Quaternion.Euler(0f, defaultRotation, 0f);
        currentRotation = defaultRotation;
        currentHeight = characterController.height;
        crouchCenter.y = -0.345f;
    }

    void Update ()
    {
        SetAnimations();
        JoystickUpdate();
        SetVelocitys();
    }


    #region Joystick

    private Joystick joystick;
    private float deathZoneX, deathZoneJumpY, deathZoneCrouchY;
    private bool xJoystickLimits, yJoystickJumpLimit, yJoystickCrouchLimit;
    
    public void StartInputs(float deathZoneX, float deathZoneJumpY, float deathZoneCrouchY, Joystick joystick)
    {
        this.deathZoneX = deathZoneX;
        this.deathZoneJumpY = deathZoneJumpY;
        this.deathZoneCrouchY = deathZoneCrouchY;
        this.joystick = joystick;
    }

    private void JoystickUpdate()
    {
        xJoystickLimits = -deathZoneX >= joystick.Horizontal || deathZoneX <= joystick.Horizontal;
        yJoystickJumpLimit = deathZoneJumpY <= joystick.Vertical;
        yJoystickCrouchLimit = -deathZoneCrouchY >= joystick.Vertical;
    }

    #endregion

    #region Animations
    private Animator animator;




    private void SetAnimations()
    {



        
        animator.SetBool("IsJumping", yJoystickJumpLimit);
        
    }

    #endregion

    #region Gravity

    private float gravityPercent;

    public void Gravity(float gravityMultiplier, float gravityMultiplierPercent, float groundGravity)
    {
        gravityPercent = (gravityMultiplier * gravityMultiplierPercent) / 100;
        animator.SetBool("IsGrounded", IsGrounded());

        if (IsGrounded() && currentDirection.y < 0.0f)
        {
            currentDirection.y = groundGravity;
            appliedMovement.y = groundGravity;
        }
        else
        {
            float previousYVelocity = currentDirection.y;
            currentDirection = currentDirection + (Physics.gravity * gravityPercent * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentDirection.y) * 0.5f, -20.0f);
        }
    }

    private bool IsGrounded() => characterController.isGrounded;

    #endregion

    #region Rotation

    private float defaultRotation = 90, currentRotation, turnSmoothVelocity;

    public void Rotation(float turnSmoothTime)
    {
        if (deathZoneX <= joystick.Horizontal)
            currentRotation = defaultRotation;
        else if (-deathZoneX >= joystick.Horizontal)
            currentRotation = 3 * defaultRotation;

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, currentRotation, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    #endregion

    #region Jump

    private float jumpPercent, jumpSpeedPercent, numberOfJumps = 0, tmpPercent = 1;
    private bool joystickJumpReady;

    public void Jump(float maxNumberOfJumps, float jumpForce, float jumpForceMultiplier, float jumpSpeed, float jumpSpeedMultiplier)
    {
        jumpPercent = (jumpForce * jumpForceMultiplier) / 100;
        jumpSpeedPercent = (jumpSpeed * jumpSpeedMultiplier) / 100;

        if (yJoystickJumpLimit && !joystickJumpReady && numberOfJumps < maxNumberOfJumps)
        {
            if (numberOfJumps == 0)
            {
                StartCoroutine(WaitForLanding());
                tmpPercent = 1;
            }
            numberOfJumps++;
            joystickJumpReady = true;

            //for (int i = 1; i <= maxNumberOfJumps; i++)
            //{
            //    if (numberOfJumps == i)
            //    {
            //        jumpPercent *= tmpPercent;
            //        tmpPercent -= 0.2f;
            //    }

            //}

            currentDirection.y = jumpPercent;
        }
        else if (!yJoystickJumpLimit)
        {
            joystickJumpReady = false;
        }


    }


    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(IsGrounded);

        numberOfJumps = 0;
    }


    #endregion

    #region Crouch

    private float currentHeight, crouchSpeedPercent;
    private Vector3 crouchCenter;

    public void Crouch(float crouchSpeed, float crouchSpeedMultiplier)
    {
        crouchSpeedPercent = (crouchSpeed * crouchSpeedMultiplier) / 100;
        animator.SetBool("IsCrouch", yJoystickCrouchLimit);

        if (yJoystickCrouchLimit)
        {
            characterController.center = crouchCenter;
            characterController.height = currentHeight * 0.7f;
        }
        else
        {
            characterController.center = Vector3.zero;
            characterController.height = currentHeight;
        }
    }

    #endregion

    #region Movement
    private float moveSpeed, moveSpeedPercent;
    private Vector3 currentDirection, moveZAxis, appliedMovement;
    
    public void Movement(float movementSpeed, float moveSpeedPercentMultiplier)
    { 
        moveSpeedPercent = (movementSpeed * moveSpeedPercentMultiplier) / 100;
        animator.SetBool("IsMoving", xJoystickLimits);

        if (transform.position.z > 0.03f)
            moveZAxis.z = -1;
        else if (transform.position.z < -0.03f)
            moveZAxis.z = 1; 
        else 
            moveZAxis.z = 0;

        characterController.Move(moveZAxis * Time.deltaTime);

        if (xJoystickLimits)
        {
            currentDirection.x = joystick.Horizontal;
            animator.SetFloat("MoveX", joystick.Horizontal);
        }
           
        else
            currentDirection.x = 0; 
        
        appliedMovement.x = currentDirection.x * moveSpeed;

        characterController.Move(appliedMovement * Time.deltaTime);
    }

    #endregion

    private void SetVelocitys()
    {
        if (IsGrounded())
        {
            moveSpeed = moveSpeedPercent;
            if (yJoystickCrouchLimit)
            {
                moveSpeed = crouchSpeedPercent;
            }
        }
        else
        {
            moveSpeed = jumpSpeedPercent;
            if (yJoystickCrouchLimit)
            {
                moveSpeed = jumpSpeedPercent;
            }
        }

    }


    
}
