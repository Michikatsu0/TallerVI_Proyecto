﻿using UnityEngine;

[RequireComponent(typeof(PositionPlatformsResponse))]

[RequireComponent(typeof(RotationPlatformsResponse))]

[RequireComponent(typeof(CircularPlatformsResponse))]
public class PlatformsController : MonoBehaviour
{
    public enum PlatformMovementType { Position, Rotation, Circular }

    [SerializeField] PlatformMovementType platformType;

    private IPositionPlatformsProvider positionPlatformsProvider;
    private IRotationPlatformProvider rotationPlatformProvider;
    private ICircularPlatformProvider circularPlatformProvider;

    private void Awake()
    {
        positionPlatformsProvider = GetComponent<IPositionPlatformsProvider>();
        rotationPlatformProvider = GetComponent<IRotationPlatformProvider>();
        circularPlatformProvider = GetComponent<ICircularPlatformProvider>();
    }

    private void FixedUpdate()
    {
        switch (platformType)
        {
            case PlatformMovementType.Position:
                positionPlatformsProvider.PositionPlatform();
                break;
            case PlatformMovementType.Rotation:
                rotationPlatformProvider.RotationPlatform();
                break;
            case PlatformMovementType.Circular:
                circularPlatformProvider.CircularPlatform();
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player")) 
        {
            other.transform.SetParent(transform);
        }
        else if (target.CompareTag("Probs"))
        {
            other.transform.SetParent(transform);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
        else if (target.CompareTag("Probs"))
        {
            other.transform.SetParent(null);
        }
    }
}

