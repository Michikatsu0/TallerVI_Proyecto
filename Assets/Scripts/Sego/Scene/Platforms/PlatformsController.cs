using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformTypes { Position, Rotation, Circular}
public enum PlatformMoveTypes { Vertical, Horizontal, Both }

[RequireComponent(typeof(PlatformsResponse), typeof(BoxCollider))]
public class PlatformsController : MonoBehaviour
{
    [SerializeField] private PlatformTypes platformType;

    [Header("Only Works for Position Movement Platforms")]
    [SerializeField] private PlatformMoveTypes platformMoveType;

    private IPlatformsProvider platformsProvider;
    private BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        platformsProvider = GetComponent<IPlatformsProvider>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (platformType)
        {
            case PlatformTypes.Position:
                platformsProvider.PositionPlatform(platformMoveType, transform.position);
                break;
            case PlatformTypes.Rotation:
                platformsProvider.RotationPlatform(transform.position);
                break;
            case PlatformTypes.Circular:
                platformsProvider.CircularPlatform(transform.position);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
