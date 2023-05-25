using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Base Mechanics Data Player", order = 1)]
public class PlayerSettings : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("Joystick Settings")]
    [SerializeField][Range(0f, 1f)] public float leftDeathZoneX;
    [SerializeField][Range(0f, 1f)] public float rightDeathZoneAimXY;
    [SerializeField][Range(0f, 1f)] public float leftDeathZoneJumpY;
    [SerializeField][Range(0f, 1f)] public float leftDeathZoneCrouchY;

    [Header("Movement Settings")]
    [SerializeField][Range(0f, 100f)] public float movementSpeedMultiplier; //cambiar
    [SerializeField] public float movementSpeed;
    [SerializeField][Range(0f, 2f)] public float xMoveSpeed;

    [Header("Rotation Settings")]
    [SerializeField][Range(0f, 0.2f)] public float turnSmoothTime;

    [Header("Crouch Movement Settings")]
    [SerializeField][Range(0f, 100f)] public float crouchSpeedMultiplier; //cambiar
    [SerializeField] public float crouchSpeed, crouchCenter;
    [SerializeField][Range(0f, 1f)] public float topHitDistance, crouchTopHitRadiusDistance;

    [Header("Gravity Settings")]
    [SerializeField][Range(0f, 100f)] public float gravityMultiplierPercentage;
    [SerializeField] public float gravityMultiplier;
    [SerializeField][Range(-1, -20)] public float groundGravity;

    [Header("Jump Settings")] //cambiar
    [SerializeField] public float maxNumberOfJumps;
    [SerializeField][Range(0f, 100f)] public float jumpForceMultiplier;
    [SerializeField] public float jumpForce;
    [SerializeField][Range(0f, 100f)] public float jumpSpeedMultiplier;
    [SerializeField] public float jumpSpeed;

    [Header("CoyoteTime Settings")]
    [SerializeField] public float coyoteTimeCounter;
    [SerializeField] public float coyoteTime;

    [Header("Fall Settings")]
    [SerializeField][Range(0f, 100f)] public float heavyFallMoveSpeedMultiplier;
    [SerializeField] public float heavyFallMoveSpeed;
    [SerializeField][Range(0f, 5f)] public float heavyFallMoveDelay, heavyFallMoveDuration;

    [SerializeField][Range(0f, 1f)] public float centerDistance;
    [SerializeField] public LayerMask isGround;

    [Header("Dash Settings")]
    [SerializeField][Range(0f, 1f)] public float dashDuration;
    [SerializeField][Range(0f, 10f)] public float dashCoolDown;
    [SerializeField][Range(0f, 100f)] public float dashForceMultiplier;
    [SerializeField] public float dashForce;
    [SerializeField] public Material dashTrailMaterial;

    [Header("Slopes Settings")]
    [SerializeField][Range(0f, 1f)] public float slopeRayDistance;
    [SerializeField] public float slideSlopeSpeed, slopeforceDown, slideSlopeMovement;
    [SerializeField][Range(0f, 1f)] public float slopeRadiusDistance;

    [Header("Push RGBD's Settings")]
    [SerializeField][Range(0f, 100f)] public float pushPowerBridgesMultiplier;
    [SerializeField] public float pushPowerBridges, pushDelay;
    [SerializeField][Range(0f, 100f)] public float pushPowerProbsMultiplier;
    [SerializeField] public float pushPowerProbs;

    [Header("Aim Speed Movement Settings")]
    [SerializeField][Range(0f, 0.2f)] public float turnLookRotationSmoothTime;
    [SerializeField][Range(0f, 10f)] public float aimAnimatorLayerSmoothTime;
    [SerializeField][Range(0f, 100f)] public float aimSpeedMultiplier;
    [SerializeField] public float aimSpeed;

    [Header("Aim Ray Settings")]
    [SerializeField] public float aimRayMaxDistance;
    [SerializeField] public float defaultWeaponRot;
    [SerializeField][Range(0f, 0.2f)] public float aimRigLayerSmoothTime;
    [SerializeField][Range(-150f, 150f)] public float weaponMultiAimRotation;

    [Header("Camera Settings")]
    [SerializeField] public float baseCamPos;
    [SerializeField] public float crouchCamPos, currentCamCrouchDelay, jumpCamPos, deadCamZone, lerpDeadZoneHeight, lerpCamMoveVelocity;

    [Header("Audio Settings")]
    [SerializeField] public List<AudioClip> footStepAudioClips = new List<AudioClip>();
    public void Init()
    {

    }
    public void OnBeforeSerialize()
    {
        Init();
    }

    public void OnAfterDeserialize()
    {

    }
}
