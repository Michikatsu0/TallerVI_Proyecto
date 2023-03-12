using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}

