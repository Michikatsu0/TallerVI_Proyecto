using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMechanicResponse : MonoBehaviour, IPlayerMechanicProvider
{
    private CinemachineVirtualCamera cam;
    private CharacterController characterController;
    private Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();

        transform.rotation = Quaternion.Euler(0f, positiveRotation, 0f);
        currentRotation = positiveRotation;

        currentHeight = characterController.height;
    }

    void Update()
    {
        JoystickUpdate();
        SetVelocitys();
    }

    #region Joystick

    private Joystick leftJoystick, rightJoystick;
    private float leftDeathZoneX, leftDeathZoneJumpY, leftDeathZoneCrouchY, rightDeathZoneAimXY;
    private bool leftJoystickXMovementLimits, leftJoystickYJumpLimit, leftJoystickYCrouchLimit, rightJoystickXYAimLimit;

    public void StartInputs(float leftDeathZoneX, float leftDeathZoneJumpY, float leftDeathZoneCrouchY, float rightDeathZoneAimXY, Joystick rightJoystick, Joystick leftJoystick)
    {
        this.leftJoystick = leftJoystick;
        this.rightJoystick = rightJoystick;

        this.leftDeathZoneX = leftDeathZoneX;
        this.leftDeathZoneJumpY = leftDeathZoneJumpY;
        this.leftDeathZoneCrouchY = leftDeathZoneCrouchY;
        this.rightDeathZoneAimXY = rightDeathZoneAimXY;
    }

    private void JoystickUpdate()
    {
        rightJoystickXYAimLimit = -rightDeathZoneAimXY >= rightJoystick.Horizontal || rightDeathZoneAimXY <= rightJoystick.Horizontal || -rightDeathZoneAimXY >= rightJoystick.Vertical || rightDeathZoneAimXY <= rightJoystick.Vertical;
        leftJoystickXMovementLimits = -leftDeathZoneX >= leftJoystick.Horizontal || leftDeathZoneX <= leftJoystick.Horizontal;
        leftJoystickYJumpLimit = leftDeathZoneJumpY <= leftJoystick.Vertical;
        leftJoystickYCrouchLimit = -leftDeathZoneCrouchY >= leftJoystick.Vertical;
    }

    #endregion

    #region Gravity

    private float gravityPercent;

    public bool IsGrounded() => characterController.isGrounded;

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

    private float fallRayDistance, currentFallTime, heavyFallMovePercent, heavyFallMoveDelay, heavyFallMoveDuration;
    private bool isFalling, canHeavyFallMove;
    private Vector3 animMovement;
    public void Fall(float centerDistance, float heavyMoveFallSpeed, float heavyMoveFallSpeedMultiplier, float heavyFallMoveDelay, float heavyFallMoveDuration, LayerMask isGround)
    {
        heavyFallMovePercent = (heavyMoveFallSpeed * heavyMoveFallSpeedMultiplier) / 100;
        this.heavyFallMoveDelay = heavyFallMoveDelay;
        this.heavyFallMoveDuration = heavyFallMoveDuration;
        
        fallRayDistance = -centerDistance;
        fallVectorDistance.y = fallRayDistance;

        isFalling = true;
        
        if (Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out RaycastHit hit, centerDistance, isGround))
        {
            isFalling = false;
        }

        animator.SetBool("IsFalling", isFalling);
        
        if (isFalling)
        {
            currentFallTime += Time.deltaTime;
            animator.SetFloat("FallTime", currentFallTime);
        }
        else
        {
            if (currentFallTime > 1.2f && !isFalling)
                StartCoroutine(HeavyFallMovement());

            if (transform.rotation.eulerAngles != Vector3.zero)
                animMovement.z = -1;
            else
                animMovement.z = 1;

            if (canHeavyFallMove)
                characterController.Move(animMovement * heavyFallMovePercent * Time.deltaTime);
            
            Invoke(nameof(ResetFallTime), 0.1f);
        }
            
    }

    private IEnumerator HeavyFallMovement()
    {
        yield return new WaitForSeconds(heavyFallMoveDelay);
        canHeavyFallMove = true;
        yield return new WaitForSeconds(heavyFallMoveDuration);
        canHeavyFallMove = false;
    }

    private void ResetFallTime()
    {
        currentFallTime = 0;
    }

    #endregion

    #region Slopes

    private float slopeRayDistance, slopeRadiusDistance;
    private RaycastHit slopeHit;

    public void SlopeSlide(float slopeRayDistance, float slopeRadiusDistance ,float slideSlopeSpeed, float slopeforceDown )
    {
        this.slopeRayDistance = slopeRayDistance;
        this.slopeRadiusDistance = slopeRadiusDistance;

        if (OnStepSlope() && !isJumping)
            SlopeMovement(slideSlopeSpeed, slopeforceDown);
    }

    private bool OnStepSlope()
    {

        if (isFalling) return false;
        if (Physics.SphereCast(transform.position, characterController.radius + slopeRadiusDistance, Vector3.down, out slopeHit, slopeRayDistance))
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

    private float positiveRotation = 0, negativeRotation = 180, currentRotation, turnSmoothVelocity;

    public void Rotation(float turnSmoothTime)
    {
        if (rightJoystickXYAimLimit || canHeavyFallMove) return;

        if (leftDeathZoneX <= leftJoystick.Horizontal)
            currentRotation = positiveRotation;
        else if (-leftDeathZoneX >= leftJoystick.Horizontal)
            currentRotation = negativeRotation;

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, currentRotation, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    #endregion

    #region Dash

    private float dashPercent, dashTime, dashDuration, dashCoolDown;
    private Vector3 joystickDashDirection;
    private bool isDashing, canDash = true;
    public void Dash(float dashDuration, float dashCoolDown, float dashForce, float dashForceMultiplier)
    {
        dashPercent = (dashForce * dashForceMultiplier) / 100;
        this.dashDuration = dashDuration;
        this.dashCoolDown = dashCoolDown;

        animator.SetBool("IsDashing", isDashing);
    }

    public void ButtonDash()
    {
        if (canDash && (leftJoystick.Horizontal != 0 || leftJoystick.Vertical != 0))
            StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        dashTime = Time.time;

        while (Time.time < dashTime + dashDuration)
        {
            joystickDashDirection.z = leftJoystick.Horizontal;
            joystickDashDirection.y = leftJoystick.Vertical;
            currentDirection = joystickDashDirection.normalized;
            isDashing = true;
            characterController.Move(currentDirection * dashPercent * Time.deltaTime);
            yield return null;
        }

        isDashing = false;

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    #endregion

    #region Jump

    private float jumpPercent, jumpSpeedPercent, maxNumberofJumps;
    private bool joystickJumpReady, isJumping;
    private int numberOfJumps = 0;

    public void Jump(float maxNumberOfJumps, float jumpForce, float jumpForceMultiplier, float jumpSpeed, float jumpSpeedMultiplier)
    {
        if (rightJoystickXYAimLimit)
            this.maxNumberofJumps = 1;
        else
            this.maxNumberofJumps = maxNumberOfJumps;


        jumpPercent = (jumpForce * jumpForceMultiplier) / 100;
        jumpSpeedPercent = (jumpSpeed * jumpSpeedMultiplier) / 100;
        CoyoteTime();

        if (canJump && leftJoystickYJumpLimit && !joystickJumpReady && numberOfJumps < maxNumberofJumps)
        {
            if (numberOfJumps == 0) StartCoroutine(WaitForLanding());

            JumpRoutine();

            currentDirection.y = jumpPercent;
        }
        else if (!leftJoystickYJumpLimit)
        {
            joystickJumpReady = false;
        }

    }

    private void JumpRoutine()
    {
        numberOfJumps++;
        joystickJumpReady = true;
        isJumping = true;
        animator.SetInteger("MultiJumps", numberOfJumps);
        animator.SetBool("IsJumping", isJumping);
        Invoke(nameof(ResetJumpAnimation), 0.1f);
        var randomJump = Random.Range(0f, 1f);
        animator.SetFloat("RandomJump", randomJump);

        currentFallTime = 0f;
    }

    private void ResetJumpAnimation()
    {
        isJumping = false;
        animator.SetBool("IsJumping", isJumping);
        if (!isFalling)
        {
            numberOfJumps = 0;
            currentFallTime = 0;
        }
            
    }

    private IEnumerator WaitForLanding()
    {
        if (!isFalling)
            numberOfJumps = 0;

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

    private float currentHeight, crouchSpeedPercent, topHitDistance, crouchRadiusDistance;
    private bool topHit;
    private Vector3 crouchCenterPosition;
    private RaycastHit hit;
    private float crouchTime; 
    public void Crouch(float crouchCenter, float crouchSpeed, float crouchSpeedMultiplier, float topHitDistance, float crouchRadiusDistance, LayerMask isGround)
    {
        this.topHitDistance = topHitDistance;
        this.crouchRadiusDistance = crouchRadiusDistance;
        crouchSpeedPercent = (crouchSpeed * crouchSpeedMultiplier) / 100;
        crouchCenterPosition.y = crouchCenter;

        topHit = false;
        animator.SetBool("IsCrouch", leftJoystickYCrouchLimit);

        if (leftJoystickYCrouchLimit)
        {
            characterController.center = crouchCenterPosition;
            characterController.height = currentHeight * 0.8f;
        }
        else 
        {
            characterController.center = Vector3.zero;
            characterController.height = currentHeight;
        }

        if (Physics.SphereCast(transform.position, characterController.radius + crouchRadiusDistance, Vector3.up, out hit, topHitDistance, isGround))
        {
            joystickJumpReady = true;
            topHit = true;
            animator.SetBool("IsCrouch", true);
            characterController.center = crouchCenterPosition;
            characterController.height = currentHeight * 0.8f;
        }
    }

    #endregion

    #region Movement

    private float moveSpeed, moveSpeedPercent;
    private Vector3 currentDirection, moveXAxis, appliedMovement;
    
    public void Movement(float movementSpeed, float moveSpeedPercentMultiplier, float xMoveSpeed)
    { 
        moveSpeedPercent = (movementSpeed * moveSpeedPercentMultiplier) / 100;
        animator.SetBool("IsMoving", leftJoystickXMovementLimits);

        if (transform.position.x > 0.03f)
            moveXAxis.x = -1;
        else if (transform.position.x < -0.03f)
            moveXAxis.x = 1; 
        else
            moveXAxis.x = 0;

        characterController.Move(moveXAxis * Time.deltaTime * xMoveSpeed);

        if (leftJoystickXMovementLimits)
            currentDirection.z = leftJoystick.Horizontal;
        else
            currentDirection.z = 0; 
        
        appliedMovement.z = currentDirection.z * moveSpeed;

        characterController.Move(appliedMovement * Time.deltaTime);
    }

    #endregion

    #region Velocitys

    private void SetVelocitys()
    {
        if (!isFalling)
        {
            if (!rightJoystickXYAimLimit)
                moveSpeed = moveSpeedPercent;
            else
                moveSpeed = aimSpeedPercent;

            if (leftJoystickYCrouchLimit || topHit)
                moveSpeed = crouchSpeedPercent;

            if (canHeavyFallMove)
                moveSpeed = 0;
        }
        else
        {
            moveSpeed = jumpSpeedPercent;
            if (leftJoystickYCrouchLimit)
                moveSpeed = jumpSpeedPercent;
        }
        if (OnStepSlope())
            moveSpeed = 2;
    }

    #endregion

    #region Aim

    private float aimSpeedPercent;

    public void Aim(float turnAimSmoothTime, float aimSpeed, float aimSpeedMultiplier)
    {
        aimSpeedPercent = (aimSpeed * aimSpeedMultiplier) / 100;

        if (rightJoystickXYAimLimit)
            animator.SetBool("IsAiming", rightJoystickXYAimLimit);
        else
            animator.SetBool("IsAiming", rightJoystickXYAimLimit);

        if (leftJoystickXMovementLimits && !rightJoystickXYAimLimit)
        {
            animator.SetFloat("MoveX", leftJoystick.Horizontal);
            animator.SetLayerWeight(1, 0);
            return;
        }
        else if (rightJoystickXYAimLimit)
            animator.SetLayerWeight(1, 1);

        if (rightDeathZoneAimXY <= rightJoystick.Horizontal)
        {
            currentRotation = positiveRotation;
            animator.SetFloat("MoveX", leftJoystick.Horizontal);
        }
        else if (-rightDeathZoneAimXY >= rightJoystick.Horizontal)
        {
            currentRotation = negativeRotation;
            animator.SetFloat("MoveX", -leftJoystick.Horizontal);
        }

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, currentRotation, ref turnSmoothVelocity, turnAimSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
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
        if (leftJoystickYCrouchLimit)
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

    private Vector3 slopeVectorDistance;
    private Vector3 fallVectorDistance;
    private Vector3 topHitVectorDistance;

    private void OnDrawGizmosSelected()
    {
        CharacterController characterController = gameObject.GetComponent<CharacterController>();
        Gizmos.color = Color.red;
        fallVectorDistance.y = fallRayDistance;
        slopeVectorDistance.y = -slopeRayDistance;
        topHitVectorDistance.y = topHitDistance;
        Gizmos.DrawRay(transform.position, slopeVectorDistance);
        Gizmos.DrawWireSphere(transform.position + slopeVectorDistance, characterController.radius + slopeRadiusDistance);
        Gizmos.DrawWireSphere(transform.position + fallVectorDistance, characterController.radius);
        Gizmos.DrawWireSphere(transform.position + topHitVectorDistance, characterController.radius + crouchRadiusDistance);
    }

    #endregion

}
