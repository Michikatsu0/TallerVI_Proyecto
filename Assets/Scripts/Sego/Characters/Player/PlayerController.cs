using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMechanicResponse))]
[RequireComponent(typeof(HealthResponse))]

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates
    {
        None,
        Stuned,
    }

    private IPlayerMechanicProvider playerMechanicsProvider;
    private HealthResponse healthResponse;
    private CharacterController characterController;
    private Animator animator;
    private RagdollResponse ragdollResponse;
    private void Awake()
    {
        ragdollResponse = GetComponent<RagdollResponse>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        healthResponse = GetComponent<HealthResponse>();
        playerMechanicsProvider = GetComponent<IPlayerMechanicProvider>();
    }

    void Update()
    {
        if (healthResponse.currentHealth <= 0 || LevelUIManager.Instance.stateGame == StatesGameLoop.Pause)
        {
            if (healthResponse.currentHealth <= 0)
                PlayerActionsResponse.ActionWeaponDeath?.Invoke(true);
            PlayerActionsResponse.ActionShootWeaponTrigger?.Invoke(false);
            animator.SetFloat("MoveX", 0);
            animator.SetBool("IsMoving", false);
            animator.enabled = false;
            return; 
        }
        else
        {
            animator.enabled = true;
        }
        playerMechanicsProvider.Gravity();
        playerMechanicsProvider.SlopeSlide();
        playerMechanicsProvider.PushObjects();
        playerMechanicsProvider.Fall();
        playerMechanicsProvider.Jump();
        playerMechanicsProvider.Crouch();
        playerMechanicsProvider.Rotation();
        playerMechanicsProvider.AimAnimationMovement();
        playerMechanicsProvider.AimRayCast();
        playerMechanicsProvider.TriggerWeapon();
        playerMechanicsProvider.Dash();
        playerMechanicsProvider.Movement();
        playerMechanicsProvider.UpdateCameraHeight();

    }


}
