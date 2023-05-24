using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine.Animations.Rigging;

public class PlayerMechanicResponse : MonoBehaviour, IPlayerMechanicProvider
{
    [Header("Player Settings")]
    [SerializeField] public PlayerSettings playerSettings;

    private CharacterController characterController;
    private Animator animator;

   public float FinalDashCd;
   public float FinalDashForce;
    public float FinalJumpNumber;
    public float FinalJumpForce;

    void Start()
    {
        FinalDashCd = playerSettings.dashCoolDown+ upgradesManager.DashCDChange;
        FinalDashForce = playerSettings.dashForce+upgradesManager.DashStrenghtChange;
        FinalJumpNumber = playerSettings.maxNumberOfJumps+upgradesManager.JumpQuantityChange;
        FinalJumpForce = playerSettings.jumpForce+upgradesManager.jumpStrenghtChange;

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        rigController = GameObject.Find("____RigLayers____").GetComponent<Animator>();

        multiAimWeaponRigPose = GameObject.Find("WeaponPose_Aiming").GetComponent<MultiAimConstraint>();

        weaponParent = GameObject.Find("Primary_Weapon").GetComponent<Transform>();
        weaponLeftGrip = GameObject.Find("| Weapon | LeftHand IK").GetComponent<Transform>();
        weaponRightGrip = GameObject.Find("| Weapon | RightHand IK").GetComponent<Transform>();
        
        aimRayCrossHair = GameObject.Find("Aim_CrossHair").transform;

        WeaponResponse existingWeapon = GetComponentInChildren<WeaponResponse>();
        if (existingWeapon)
            Equip(existingWeapon);

        virtualCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        transform.rotation = Quaternion.Euler(0f, positiveRotation, 0f);
        currentRotation = positiveRotation;

        currentHeight = characterController.height;

        trailRenderer = GetComponentInChildren<TrailRenderer>();
        trailRenderer.material = playerSettings.dashTrailMaterial;
        trailRenderer.startWidth = 1.3f;
        trailRenderer.endWidth = 1;
        trailRenderer.time = playerSettings.dashDuration; //here is dash duration

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        JoystickUpdate();
        SetVelocitys();
        AimingDownUpdate();
    }

    #region Camara 

    private CinemachineFramingTransposer framingTransposer;
    private CinemachineVirtualCamera virtualCamera;
    private Vector3 currentCamPos;
    private float currentCamCrouchTime;

    public void UpdateCameraHeight()
    {
        if (isFalling)
        {
            currentCamPos.y = playerSettings.jumpCamPos;
            framingTransposer.m_DeadZoneHeight = Mathf.Lerp(framingTransposer.m_DeadZoneHeight, playerSettings.deadCamZone, 0.5f);
        }
        else
        {
            currentCamPos.y = playerSettings.baseCamPos;
            framingTransposer.m_DeadZoneHeight = Mathf.Lerp(framingTransposer.m_DeadZoneHeight, 0, 0.5f);

            if (leftJoystickYCrouchLimit)
            {
                if (leftJoystick.Vertical <= -0.9f)
                {
                    currentCamCrouchTime += Time.deltaTime;
                    if (currentCamCrouchTime > playerSettings.currentCamCrouchDelay)
                        currentCamPos.y = playerSettings.crouchCamPos;
                }
                else
                    currentCamCrouchTime = 0;
            }
        }
        framingTransposer.m_TrackedObjectOffset = currentCamPos;
    }

    #endregion

    #region Joystick

    [Header("Joystick Settings")]
    [SerializeField] private Joystick leftJoystick;
    [SerializeField] private Joystick rightJoystick;
    private bool leftJoystickXMovementLimits, leftJoystickYJumpLimit, leftJoystickYCrouchLimit, rightJoystickXYAimLimit;

    private void JoystickUpdate()
    {
        rightJoystickXYAimLimit = -playerSettings.rightDeathZoneAimXY >= rightJoystick.Horizontal || playerSettings.rightDeathZoneAimXY <= rightJoystick.Horizontal || -playerSettings.rightDeathZoneAimXY >= rightJoystick.Vertical || playerSettings.rightDeathZoneAimXY <= rightJoystick.Vertical;
        leftJoystickXMovementLimits = -playerSettings.leftDeathZoneX >= leftJoystick.Horizontal || playerSettings.leftDeathZoneX <= leftJoystick.Horizontal;
        leftJoystickYJumpLimit = playerSettings.leftDeathZoneJumpY <= leftJoystick.Vertical;
        leftJoystickYCrouchLimit = -playerSettings.leftDeathZoneCrouchY >= leftJoystick.Vertical;
    }

    #endregion

    #region Gravity

    private float gravityPercent;

    public bool IsGrounded() => characterController.isGrounded;

    public void Gravity()
    {
        gravityPercent = (playerSettings.gravityMultiplier * playerSettings.gravityMultiplierPercentage) / 100;
        animator.SetBool("IsGrounded", IsGrounded());
        if (IsGrounded() && currentDirection.y < 0.0f)
        {
            currentDirection.y = playerSettings.groundGravity;
            appliedMovement.y = playerSettings.groundGravity;
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

    private float currentFallTime, heavyFallMovePercent;
    private bool isFalling, canHeavyFallMove;
    private Vector3 animMovement;

    public void Fall()
    {
        heavyFallMovePercent = (playerSettings.heavyFallMoveSpeed * playerSettings.heavyFallMoveSpeedMultiplier) / 100;

        isFalling = true;
        
        if (Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out RaycastHit hit, playerSettings.centerDistance, playerSettings.isGround))
        {
            isFalling = false;
        }

        animator.SetBool("IsFalling", isFalling);
        
        if (isFalling)
        {
            currentFallTime += Time.deltaTime;
            animator.SetFloat("FallTime", currentFallTime);
            currentCamPos.y = 0f;
        }
        else
        {
            if (currentFallTime > 1.2f && !isFalling)
                StartCoroutine(HeavyFallMovement());

            if (leftJoystick.Horizontal != 0)
            {
                if (rightJoystickXYAimLimit)
                    animMovement.z = rightJoystick.Horizontal;
                else
                    animMovement.z = leftJoystick.Horizontal;
            }
            else
            {
                if (rightJoystickXYAimLimit)
                    animMovement.z = rightJoystick.Horizontal;
                else
                {
                    if (transform.rotation.eulerAngles.y < 90f)
                        animMovement.z = 1f;
                    else
                        animMovement.z = -1f;
                }
            }

            if (canHeavyFallMove)
                characterController.Move(animMovement.normalized * heavyFallMovePercent * Time.deltaTime);

            currentFallTime = 0;
        }
    }

    private IEnumerator HeavyFallMovement()
    {
        yield return new WaitForSeconds(playerSettings.heavyFallMoveDelay);
        canHeavyFallMove = true;
        yield return new WaitForSeconds(playerSettings.heavyFallMoveDuration);
        canHeavyFallMove = false;
    }

    #endregion

    #region Slopes

    private RaycastHit slopeHit;
    public void SlopeSlide()
    {
        if (OnStepSlope() && !isJumping)
        {
            Vector3 slopeDirection = Vector3.up - slopeHit.normal * Vector3.Dot(Vector3.up, slopeHit.normal);
            float slideSpeed = playerSettings.slideSlopeSpeed * Time.deltaTime;

            currentDirection = slopeDirection * -slideSpeed;
            currentDirection.y = -playerSettings.slopeforceDown * Time.deltaTime;

            characterController.Move(currentDirection);
        }
    }

    private bool OnStepSlope()
    {
        if (isFalling) return false;

        if (Physics.SphereCast(transform.position, characterController.radius + playerSettings.slopeRadiusDistance, Vector3.down, out slopeHit, playerSettings.slopeRayDistance))
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

    #endregion

    #region Rotation

    private float positiveRotation = 0, negativeRotation = 180, currentRotation, turnSmoothVelocity;

    public void Rotation()
    {
        if (rightJoystickXYAimLimit) return;

        if (playerSettings.leftDeathZoneX <= leftJoystick.Horizontal)
            currentRotation = positiveRotation;
        else if (-playerSettings.leftDeathZoneX >= leftJoystick.Horizontal)
            currentRotation = negativeRotation;
        else if (isDashing)
        {
            if (joystickDashDirection.z > 0)
                currentRotation = positiveRotation;
            else if (joystickDashDirection.z < 0)
                currentRotation = negativeRotation;
        }

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, currentRotation, ref turnSmoothVelocity, playerSettings.turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    #endregion

    #region Dash

    private float dashPercent, dashTime;
    private Vector3 joystickDashDirection;
    private bool isDashing, canDash = true;
    private TrailRenderer trailRenderer;

    public void Dash()
    {
        dashPercent = (FinalDashForce * playerSettings.dashForceMultiplier) / 100; //change the dash strenght

        animator.SetBool("IsDashing", isDashing);

        PlayerActionsResponse.ActionDashBarCoolDown?.Invoke(isDashing);
        trailRenderer.emitting = isDashing;
    }

    public void ButtonDash()
    {
        if (canDash && (leftJoystick.Horizontal != 0 || leftJoystick.Vertical != 0) )
            StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        canDash = false;
        dashTime = Time.time;

        while (Time.time < dashTime + playerSettings.dashDuration)
        {

            joystickDashDirection.z = leftJoystick.Horizontal;
            joystickDashDirection.y = leftJoystick.Vertical;

            currentDirection = joystickDashDirection.normalized;
            isDashing = true;
            characterController.Move(currentDirection * dashPercent * Time.deltaTime);
            yield return null;
        }
        dashTime = 0;
        isDashing = false;
        yield return new WaitForSeconds(FinalDashCd);
        canDash = true;
        statsManager.dashesDados++;
    }

    #endregion

    #region Jump

    private float jumpPercent, jumpSpeedPercent, maxNumberofJumps;
    private bool joystickJumpReady, isJumping, canJump;
    private int numberOfJumps = 0;

    public void Jump()
    {
        if (rightJoystickXYAimLimit)
            this.maxNumberofJumps = 1;
        else
            this.maxNumberofJumps = FinalJumpNumber; //jump number

        jumpPercent = (FinalJumpForce * playerSettings.jumpForceMultiplier) / 100; //here are the jump settings to change of force
        jumpSpeedPercent = (playerSettings.jumpSpeed * playerSettings.jumpSpeedMultiplier) / 100;
        
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
        statsManager.saltosDados++; //cada que salta añade uno al contador
    }

    private void CoyoteTime()
    {
        if (isFalling && numberOfJumps == 0)
            playerSettings.coyoteTimeCounter -= Time.deltaTime;
        else
            playerSettings.coyoteTimeCounter = playerSettings.coyoteTime;

        if (playerSettings.coyoteTimeCounter > 0)
            canJump = true;
        else
            canJump = false;
    }

    private void JumpRoutine()
    {
        numberOfJumps++;
        joystickJumpReady = true;
        isJumping = true;
        animator.SetInteger("MultiJumps", numberOfJumps);
        animator.SetBool("IsJumping", isJumping);
        Invoke(nameof(ResetJumpAnimation), 0.1f);
        var randomJump = UnityEngine.Random.Range(0, numberOfJumps);
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

    #region Crouch

    private float currentHeight, crouchSpeedPercent;
    private bool topHit;
    private Vector3 crouchCenterPosition;
    private RaycastHit topHitCrouch;

    public void Crouch()
    {
        crouchSpeedPercent = (playerSettings.crouchSpeed * playerSettings.crouchSpeedMultiplier) / 100;
        crouchCenterPosition.y = playerSettings.crouchCenter;

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
            if (canHeavyFallMove)
            {
                characterController.center = crouchCenterPosition;
                characterController.height = currentHeight * 0.8f;
            }

        }

        if (Physics.SphereCast(transform.position, characterController.radius + playerSettings.crouchTopHitRadiusDistance, Vector3.up, out topHitCrouch, playerSettings.topHitDistance, playerSettings.isGround))
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
    
    public void Movement()
    { 
        moveSpeedPercent = (playerSettings.movementSpeed * playerSettings.movementSpeedMultiplier) / 100;
        animator.SetBool("IsMoving", leftJoystickXMovementLimits);

        if (transform.position.x > 0.03f)
            moveXAxis.x = -1;
        else if (transform.position.x < -0.03f)
            moveXAxis.x = 1; 
        else
            moveXAxis.x = 0;

        characterController.Move(moveXAxis * Time.deltaTime * playerSettings.xMoveSpeed);

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
            moveSpeed = playerSettings.slideSlopeMovement;
    }

    #endregion

    #region Aim & Rotation Movement

    private float aimSpeedPercent, aimingAllowedAngle, refVelocity, multiWeaponPoseOffset;
    private Vector3 aimWeaponRotation;
    private MultiAimConstraint multiAimWeaponRigPose;

    public void AimingDownUpdate()
    {
        aimingAllowedAngle = Mathf.Atan2(rightJoystick.Vertical, rightJoystick.Horizontal) * Mathf.Rad2Deg;

        if (aimingAllowedAngle >= -125 && aimingAllowedAngle <= -55 || aimingAllowedAngle >= 235 && aimingAllowedAngle <= 305)
            multiWeaponPoseOffset = playerSettings.weaponMultiAimRotation * (1 - Mathf.Abs(rightJoystick.Horizontal));
        else
            multiWeaponPoseOffset = 0;

        aimWeaponRotation.z = Mathf.SmoothDamp(aimWeaponRotation.z, multiWeaponPoseOffset, ref refVelocity, 0.04f);

        multiAimWeaponRigPose.data.offset = aimWeaponRotation;
    }

    public void AimAnimationMovement()
    {
        aimSpeedPercent = (playerSettings.aimSpeed * playerSettings.aimSpeedMultiplier) / 100;

        if (playerSettings.rightDeathZoneAimXY <= rightJoystick.Horizontal)
        {
            currentRotation = positiveRotation;
            if (isDashing)
            {
                if (joystickDashDirection.z > 0)
                    currentRotation = positiveRotation;
                else if (joystickDashDirection.z < 0)
                    currentRotation = negativeRotation;
            } 
                
            animator.SetFloat("MoveX", leftJoystick.Horizontal);
        }
        else if (-playerSettings.rightDeathZoneAimXY >= rightJoystick.Horizontal)
        {
            currentRotation = negativeRotation;
            if (isDashing)
            {
                if (joystickDashDirection.z > 0)
                    currentRotation = positiveRotation;
                else if (joystickDashDirection.z < 0)
                    currentRotation = negativeRotation;
            }
            animator.SetFloat("MoveX", -leftJoystick.Horizontal);
        }

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, currentRotation, ref turnSmoothVelocity, playerSettings.turnLookRotationSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

    }

    #endregion

    #region WeaponActive

    private float animatorLayer1, animatorLayer2, animatorLayer3;

    private Animator rigController;
    private WeaponResponse currentWeapon;
    private Transform weaponParent;
    private Transform weaponLeftGrip;
    private Transform weaponRightGrip;

    public void TriggerWeapon()
    {
        if (currentWeapon)
        {
            animatorLayer1 = Mathf.Lerp(animator.GetLayerWeight(1), 0, playerSettings.aimAnimatorLayerSmoothTime);

            PlayerActionsResponse.ActionShootWeaponTrigger?.Invoke(rightJoystickXYAimLimit);

            if (rightJoystickXYAimLimit)
            {
                animatorLayer2 = Mathf.Lerp(animator.GetLayerWeight(2), 1, playerSettings.aimAnimatorLayerSmoothTime);
                
            }
            else
            {
                animatorLayer2 = Mathf.Lerp(animator.GetLayerWeight(2), 0, playerSettings.aimAnimatorLayerSmoothTime);


                if (leftJoystickXMovementLimits)
                    animator.SetFloat("MoveX", leftJoystick.Horizontal);
            }
            animator.SetLayerWeight(1, animatorLayer1);
            animator.SetLayerWeight(2, animatorLayer2);

        }
        else
        {

            animatorLayer2 = Mathf.Lerp(animator.GetLayerWeight(2), 0, playerSettings.aimAnimatorLayerSmoothTime);
           

            if (rightJoystickXYAimLimit)
            {
                animatorLayer1 = Mathf.Lerp(animator.GetLayerWeight(1), 1, playerSettings.aimAnimatorLayerSmoothTime);
               
            }
            else
            {
                animatorLayer1 = Mathf.Lerp(animator.GetLayerWeight(1), 0, playerSettings.aimAnimatorLayerSmoothTime);


                if (leftJoystickXMovementLimits)
                    animator.SetFloat("MoveX", leftJoystick.Horizontal);

            }
            animator.SetLayerWeight(1, animatorLayer1);
            animator.SetLayerWeight(2, animatorLayer2);
        }
    }

    public void Equip(WeaponResponse nextWeapon)
    {
        if (currentWeapon) { 
            Destroy(currentWeapon.gameObject);
        }
        currentWeapon = nextWeapon;
        currentWeapon.raycastDestination = aimRayCrossHair;
        currentWeapon.transform.SetParent(weaponParent);
        currentWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    #endregion

    #region Aim Raycast

    private Vector3 aimDirection, displacement;
    private Ray aimRay;
    private RaycastHit aimHit;
    private Transform aimRayCrossHair;

    public void AimRayCast()
    {
        aimDirection.z = rightJoystick.Horizontal;
        aimDirection.y = rightJoystick.Vertical;

        aimRay.origin = transform.position;
        aimRay.direction = aimDirection;

        if (Physics.Raycast(aimRay, out aimHit, playerSettings.aimRayMaxDistance))
        {
            if (aimHit.collider.gameObject.layer == LayerMask.NameToLayer("IsGround"))
            {
                Debug.DrawRay(aimRay.origin, aimRay.direction * aimHit.distance, Color.red);
                aimRayCrossHair.transform.position = aimHit.point;
            }
        }
        else
        {
            Debug.DrawRay(aimRay.origin, aimRay.direction * playerSettings.aimRayMaxDistance, Color.red);

            if (aimRay.direction.magnitude != 0)
                displacement = aimRay.direction.normalized * playerSettings.aimRayMaxDistance;
            else
            {
                if (currentRotation == positiveRotation)
                    displacement = Vector3.forward * playerSettings.aimRayMaxDistance / 10f;
                else
                    displacement = -Vector3.forward * playerSettings.aimRayMaxDistance / 10f;
            }
            aimRayCrossHair.transform.position = transform.position + displacement;
        }
    }

    #endregion

    #region Audio 

    private AudioSource audioSource;
    private float currentVolumen;
   

    private void OnFootStep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (playerSettings.footStepAudioClips.Count > 0)
            {
                var index = Random.Range(0, playerSettings.footStepAudioClips.Count);
                if (animationEvent.intParameter == 0)
                    currentVolumen = 0.25f;
                else if (animationEvent.intParameter == 1)
                    currentVolumen = 0.4f;
                else if (animationEvent.intParameter == 2)
                    currentVolumen = 0.7f;
                else
                    currentVolumen = 0.15f;
                AudioSource.PlayClipAtPoint(playerSettings.footStepAudioClips[index], transform.TransformPoint(new Vector3(0.0f, characterController.center.y - characterController.height / 2, 0.0f)), currentVolumen);
            }
        }
    }

    //private void OnLand(AnimationEvent animationEvent)
    //{
    //    if (animationEvent.animatorClipInfo.weight > 0.5f)
    //    {
    //        AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
    //    }
    //}

    #endregion

    #region Collider Character Controller

    private Vector3 pushBridgesV, pushProbsV;
    private float pushPowerBridgesPercent, pushPowerProbsPercent, pushDelay, pushTime = 0;

    public void PushObjects()
    {
        if (-0.6f < leftJoystick.Horizontal && 0.6f > leftJoystick.Horizontal)
            pushPowerBridgesPercent = 150;
        else
            pushPowerBridgesPercent = (playerSettings.pushPowerBridges * playerSettings.pushPowerBridgesMultiplier) / 100;
        if (leftJoystickYCrouchLimit)
            pushPowerBridgesPercent = 120;

        pushPowerProbsPercent = (playerSettings.pushPowerProbs * playerSettings.pushPowerProbsMultiplier) / 100;
        this.pushDelay = playerSettings.pushDelay;
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
            if (currentDirection.z == 0) return;

            if (pushTime >= pushDelay)
            {
                Debug.Log("eche q: " + pushPowerBridgesPercent);
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
            if (hit.moveDirection.x < 0 || 0 < hit.moveDirection.x) return;


            pushProbsV.z = hit.moveDirection.z;
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
        fallVectorDistance.y = -playerSettings.centerDistance; 
        slopeVectorDistance.y = -playerSettings.slopeRayDistance;
        topHitVectorDistance.y = playerSettings.topHitDistance;
        Gizmos.DrawRay(transform.position, slopeVectorDistance);
        Gizmos.DrawWireSphere(transform.position + slopeVectorDistance, characterController.radius + playerSettings.slopeRadiusDistance);
        Gizmos.DrawWireSphere(transform.position + fallVectorDistance, characterController.radius);
        Gizmos.DrawWireSphere(transform.position + topHitVectorDistance, characterController.radius + playerSettings.crouchTopHitRadiusDistance);
    }

    #endregion

}
