using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMechanicResponse))]
[RequireComponent(typeof(HealthResponse))]

public class PlayerController : MonoBehaviour
{
    private IPlayerMechanicProvider playerMechanicsProvider;
    private HealthResponse healthResponse;
    private void Awake()
    {
        healthResponse = GetComponent<HealthResponse>();
        playerMechanicsProvider = GetComponent<IPlayerMechanicProvider>();
    }

    void Update()
    {
        if (healthResponse.currentHealth <= 0) return;

        playerMechanicsProvider.Gravity();
        playerMechanicsProvider.SlopeSlide();
        playerMechanicsProvider.PushObjects();
        playerMechanicsProvider.Fall();
        playerMechanicsProvider.Jump();
        playerMechanicsProvider.Crouch();
        playerMechanicsProvider.Rotation();
        playerMechanicsProvider.AimAnimationMovement();
        playerMechanicsProvider.AimRayCast();
        playerMechanicsProvider.Dash();
        playerMechanicsProvider.Movement();
        playerMechanicsProvider.UpdateCameraHeight();
    }
    private void FixedUpdate()
    {

    }
}
