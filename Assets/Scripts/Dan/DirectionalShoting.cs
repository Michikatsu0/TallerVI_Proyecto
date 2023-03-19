using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalShoting : MonoBehaviour
{
    [Header("Joystick Settings")]
    [SerializeField] public Joystick joystick;
    [SerializeField][Range(0f, 1f)] public float deathZoneX;
    [SerializeField][Range(0f, 1f)] public float deathZoneJumpY;
    [SerializeField][Range(0f, 1f)] public float deathZoneCrouchY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
