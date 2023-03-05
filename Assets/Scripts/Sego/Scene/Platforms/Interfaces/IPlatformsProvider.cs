using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformsProvider
{
    void PositionPlatform(PlatformMoveTypes platformType, Vector3 position);
    void RotationPlatform(Vector3 rotation);
    void CircularPlatform(Vector3 position);
}
