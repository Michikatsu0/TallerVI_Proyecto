using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMechanicResponse))]
[RequireComponent(typeof(HealthResponse))]

public class PlayerController : MonoBehaviour
{
    private IPlayerMechanicProvider playerMechanicsProvider;

    private void Awake()
    {
        playerMechanicsProvider = GetComponent<IPlayerMechanicProvider>();
    }

    void Update()
    {
        playerMechanicsProvider.Gravity();
        playerMechanicsProvider.SlopeSlide();
        playerMechanicsProvider.PushObjects();
        playerMechanicsProvider.Fall();
        playerMechanicsProvider.Jump();
        playerMechanicsProvider.Crouch();
        playerMechanicsProvider.Rotation();
        playerMechanicsProvider.AimAnimationMovement();
        playerMechanicsProvider.Dash();
        playerMechanicsProvider.Movement();
        playerMechanicsProvider.UpdateCameraHeight();
    }

}
