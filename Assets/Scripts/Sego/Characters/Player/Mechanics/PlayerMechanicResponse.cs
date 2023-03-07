using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanicResponse : MonoBehaviour, IPlayerMechanicProvider
{
    [Header("Player Movement Settings")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private float runSpeed;
    [SerializeField] [Range(0f,1f)] private float deathZone;
    private CharacterController characterController;
    private Vector2 direction = Vector3.zero;

    [Header("Player Rotation Settings")]
    [SerializeField] private float smooth;

    [Header("Player Jump & Gravity Settings")]
    [SerializeField] private float jumpForce;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void JumpAndGravity()
    {
        throw new System.NotImplementedException();
    }

    public void Movement()
    {
        direction.x = joystick.Horizontal * runSpeed;

        if (joystick.Horizontal >= deathZone)
            characterController.Move(direction * Time.deltaTime);
    }

    public void Rotation()
    {
        throw new System.NotImplementedException();
    }

    
}
