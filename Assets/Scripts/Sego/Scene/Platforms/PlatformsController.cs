using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformTypes { Position, Rotation, Circular}
public enum PlatformMoveTypes { Vertical, Horizontal, Both }

[RequireComponent(typeof(PlatformsResponse))]
public class PlatformsController : MonoBehaviour
{
    [SerializeField] private PlatformTypes platformType;

    [Header("Only Works for Position Movement Platforms")]
    [SerializeField] private PlatformMoveTypes platformMoveType;

    private IPlatformsProvider platformsProvider;
    [SerializeField] private BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        platformsProvider = GetComponent<IPlatformsProvider>();
        if (boxCollider != null)
            boxCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (platformType)
        {
            case PlatformTypes.Position:
                platformsProvider.PositionPlatform(platformMoveType);
                break;
            case PlatformTypes.Rotation:
                platformsProvider.RotationPlatform();
                break;
            case PlatformTypes.Circular:
                platformsProvider.CircularPlatform();
                break;
        }
    }

}
