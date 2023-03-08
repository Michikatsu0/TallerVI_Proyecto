using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private float runSpeed;
    [SerializeField][Range(0f, 1f)] private float deathZone;
    private Vector2 direction = Vector3.zero;

    [Header("Player Rotation Settings")]
    [SerializeField] private float turnSmoothTime;

    [Header("Player Jump & Gravity Settings")]
    [SerializeField] private float jumpForce;

    private IPlayerMechanicProvider playerMechanicsProvider;
    void Start()
    {
        playerMechanicsProvider = GetComponent<IPlayerMechanicProvider>();
    }

    // Update is called once per frame
    void Update()
    {
        playerMechanicsProvider.Movement(runSpeed, deathZone, turnSmoothTime, direction, joystick);
    }
}
