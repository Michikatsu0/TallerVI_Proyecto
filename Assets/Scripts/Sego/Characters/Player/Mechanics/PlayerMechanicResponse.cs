using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMechanicResponse : MonoBehaviour, IPlayerMechanicProvider
{
    private CharacterController characterController;
    private Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        transform.rotation = Quaternion.Euler(0f, defaultRotation, 0f);
        currentRotation = defaultRotation;
        currentHeight = characterController.height;
        crouchCenter.y = -0.16f;
    }

    void Update()
    {
        JoystickUpdate();
        SetVelocitys();
    }

    #region Joystick
    
    private Joystick rightJoystick, leftJoystick;
    private float deathZoneX, deathZoneJumpY, deathZoneCrouchY, deathZoneAimXY;
    private bool xJoystickLimits, yJoystickJumpLimit, yJoystickCrouchLimit, xyJoystickAimLimit;

    public void StartInputs(float deathZoneX, float deathZoneJumpY, float deathZoneCrouchY, float deathZoneAimXY,  Joystick rightJoystick, Joystick leftJoystick)
    {
        this.deathZoneX = deathZoneX;
        this.deathZoneJumpY = deathZoneJumpY;
        this.deathZoneCrouchY = deathZoneCrouchY;
        this.deathZoneAimXY = deathZoneAimXY;
        this.rightJoystick = rightJoystick;
        this.leftJoystick = leftJoystick;
    }

    private void JoystickUpdate()
    {
        xyJoystickAimLimit = -deathZoneAimXY >= rightJoystick.Horizontal || deathZoneAimXY <= rightJoystick.Horizontal || -deathZoneAimXY >= rightJoystick.Vertical || deathZoneAimXY <= rightJoystick.Vertical;
        xJoystickLimits = -deathZoneX >= leftJoystick.Horizontal || deathZoneX <= leftJoystick.Horizontal;
        yJoystickJumpLimit = deathZoneJumpY <= leftJoystick.Vertical;
        yJoystickCrouchLimit = -deathZoneCrouchY >= leftJoystick.Vertical;
    }

    #endregion

    #region Gravity

    private float gravityPercent;

    private bool IsGrounded() => characterController.isGrounded;

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

    #endregion

    #region Falling

    private float tmpDistance;
    private bool isFalling;

    public void Fall(float centerDistance, LayerMask isGround)
    {
        tmpDistance = -centerDistance;
        vDistance.y = tmpDistance;

        isFalling = true;
        if (Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out RaycastHit hit, centerDistance, isGround))
            isFalling = false;

        animator.SetBool("IsFalling", isFalling);
    }

    #endregion

    #region Slopes

    private float slopeRayDistance;
    private RaycastHit slopeHit;

    public void SlopeSlide(float slopeRayDistance, float slideSlopeSpeed, float slopeforceDown )
    {
        this.slopeRayDistance = slopeRayDistance;

        if (OnStepSlope() && !jumping)
            SlopeMovement(slideSlopeSpeed, slopeforceDown);
    }

    private bool OnStepSlope()
    {

        if (isFalling) return false;
        if (Physics.SphereCast(transform.position, characterController.radius+0.03f, Vector3.down, out slopeHit, slopeRayDistance))
        {
            Collider slope = slopeHit.collider;
            GameObject targetSlope = slope.gameObject;

            if (targetSlope.CompareTag("Slopes"))
            {
                float slopeAngle = Vector3.Angle(slopeHit.normal, Vector3.up);
                if (slopeAngle > characterController.slopeLimit)
                    return true;
            }
            return false;
        }
        return false;
    }

    private void SlopeMovement(float slideSlopeSpeed, float slopeforceDown)
    {
        Vector3 slopeDirection = Vector3.up - slopeHit.normal * Vector3.Dot(Vector3.up, slopeHit.normal);
        float slideSpeed = slideSlopeSpeed * Time.deltaTime;

        currentDirection = slopeDirection * -slideSpeed;
        currentDirection.y = -slopeforceDown * Time.deltaTime;

        characterController.Move(currentDirection);
    }

    #endregion

    #region Rotation

    private float defaultRotation = 90, currentRotation, turnSmoothVelocity;
    public void Rotation(float turnSmoothTime)
    {
        if (xyJoystickAimLimit) return;

        if (deathZoneX <= leftJoystick.Horizontal)
            currentRotation = defaultRotation;
        else if (-deathZoneX >= leftJoystick.Horizontal)
            currentRotation = 3 * defaultRotation;

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, currentRotation, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    #endregion

    #region Jump

    private float jumpPercent, jumpSpeedPercent, maxNumberofJumps;
    private bool joystickJumpReady, jumping;
    private int numberOfJumps = 0;

    public void Jump(float maxNumberOfJumps, float jumpForce, float jumpForceMultiplier, float jumpSpeed, float jumpSpeedMultiplier)
    {
        if (xyJoystickAimLimit)
            this.maxNumberofJumps = 1;
        else
            this.maxNumberofJumps = maxNumberOfJumps;


        jumpPercent = (jumpForce * jumpForceMultiplier) / 100;
        jumpSpeedPercent = (jumpSpeed * jumpSpeedMultiplier) / 100;
        CoyoteTime();

        if (canJump && yJoystickJumpLimit && !joystickJumpReady && numberOfJumps < maxNumberofJumps)
        {
            if (numberOfJumps == 0) StartCoroutine(WaitForLanding());
            
            numberOfJumps++;
            joystickJumpReady = true;
            jumping = true;
            animator.SetInteger("MultiJumps", numberOfJumps);
            animator.SetBool("IsJumping", jumping);
            Invoke(nameof(ResetJumpAnimation), 0.1f);
            var randomJump = Random.Range(0f, 1f);
            animator.SetFloat("RandomJump", randomJump);
            
            currentDirection.y = jumpPercent;
        }
        else if (!yJoystickJumpLimit)
        {
            joystickJumpReady = false;
        }

    }

    private void ResetJumpAnimation()
    {
        jumping = false;
        animator.SetBool("IsJumping", jumping);
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => isFalling);
        yield return new WaitUntil(() => !isFalling);

        numberOfJumps = 0;
    }

    #endregion

    #region Coyote Time

    private bool canJump;
    private float coyoteTime = 0.2f, coyoteTimeCounter = 0.5f;

    private void CoyoteTime()
    {
        if (isFalling && numberOfJumps == 0)
            coyoteTimeCounter -= Time.deltaTime;
        else
            coyoteTimeCounter = coyoteTime;

        if (coyoteTimeCounter > 0)
            canJump = true;
        else
            canJump = false;
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
            characterController.height = currentHeight * 0.8f;
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
            currentDirection.x = leftJoystick.Horizontal;
        else
            currentDirection.x = 0; 
        
        appliedMovement.x = currentDirection.x * moveSpeed;

        characterController.Move(appliedMovement * Time.deltaTime);
    }

    #endregion
    private float aimSpeedPercent;

    public void Aim(float turnAimSmoothTime, float aimSpeed, float aimSpeedMultiplier)
    {
        aimSpeedPercent = (aimSpeed * aimSpeedMultiplier) / 100;

        if (xyJoystickAimLimit)
            animator.SetBool("IsAiming", xyJoystickAimLimit);
        else
            animator.SetBool("IsAiming", xyJoystickAimLimit);

        if (xJoystickLimits && !xyJoystickAimLimit)
        {
            animator.SetFloat("MoveX", leftJoystick.Horizontal);
            animator.SetLayerWeight(1, 0);
            return;
        }
        else if (xyJoystickAimLimit)
            animator.SetLayerWeight(1, 1);

        if (deathZoneAimXY <= rightJoystick.Horizontal)
        {
            currentRotation = defaultRotation;
            animator.SetFloat("MoveX", leftJoystick.Horizontal);
        }
        else if (-deathZoneAimXY >= rightJoystick.Horizontal)
        {
            currentRotation = 3 * defaultRotation;
            animator.SetFloat("MoveX", -leftJoystick.Horizontal);
        }

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, currentRotation, ref turnSmoothVelocity, turnAimSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }


    #region Velocitys

    private void SetVelocitys()
    {
        if (IsGrounded())
        {
            if (!xyJoystickAimLimit)
                moveSpeed = moveSpeedPercent;
            else
                moveSpeed = aimSpeedPercent;

            if (yJoystickCrouchLimit)
                moveSpeed = crouchSpeedPercent;
            
        }
        else
        {
            moveSpeed = jumpSpeedPercent;
            if (yJoystickCrouchLimit)
                moveSpeed = jumpSpeedPercent;
            
        }
        if (OnStepSlope())
            moveSpeed = 2;
    }

    #endregion

    #region Collider Character Controller

    private Vector3 pushBridgesV, pushProbsV;
    private float pushPowerBridgesPercent, pushPowerProbsPercent, pushDelay, pushTime = 0;

    public void PushObjects(float pushPowerBridges, float pushPowerBridgesMultiplier, float pushDelay, float pushPowerProbs, float pushPowerProbsMultiplier)
    {
        if (-0.6f < leftJoystick.Horizontal && 0.6f > leftJoystick.Horizontal)
            pushPowerBridgesPercent = 70;
        else
            pushPowerBridgesPercent = (pushPowerBridges * pushPowerBridgesMultiplier) / 100;
        if (yJoystickCrouchLimit)
            pushPowerBridgesPercent = 50;

        pushPowerProbsPercent = (pushPowerProbs * pushPowerProbsMultiplier) / 100;
        this.pushDelay = pushDelay;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject target = hit.gameObject;
        if (target.CompareTag("Bridges"))
        {
            Rigidbody rgbd = target.GetComponent<Rigidbody>();
            if (rgbd == null || rgbd.isKinematic) return;
            if (hit.moveDirection.y > 0) return;
            if (hit.moveDirection.x < 0 || 0 < hit.moveDirection.x || hit.moveDirection.z < 0 || 0 < hit.moveDirection.z) return;
            if (currentDirection.x == 0) return;

            if (pushTime >= pushDelay)
            {
                pushBridgesV.y = hit.moveDirection.y * pushPowerBridgesPercent / rgbd.mass;
                rgbd.AddForce(pushBridgesV, ForceMode.Force);
                pushTime = 0;
            }
            pushTime += Time.deltaTime;
        }
        else if (target.CompareTag("Probs"))
        {
            Rigidbody rgbd = target.GetComponent<Rigidbody>();
            if (rgbd == null || rgbd.isKinematic) return;
            if (hit.moveDirection.y < 0 || 0 < hit.moveDirection.y) return;
            if (hit.moveDirection.z < 0 || 0 < hit.moveDirection.z) return;


            pushProbsV.x = hit.moveDirection.x ;
            rgbd.velocity = pushProbsV * (pushPowerProbsPercent) / rgbd.mass;
        }
    }

    #endregion

    #region Gizmos
    private Vector3 target;
    private Vector3 vDistance;
    private void OnDrawGizmosSelected()
    {
        CharacterController characterController = gameObject.GetComponent<CharacterController>();
        Gizmos.color = Color.red;
        vDistance.y = tmpDistance;
        target.y = -slopeRayDistance;
        Gizmos.DrawRay(transform.position, target);
        Gizmos.DrawWireSphere(transform.position + vDistance, characterController.radius);
        Gizmos.DrawWireSphere(transform.position + target, characterController.radius + 0.03f);
    }
    #endregion

}
