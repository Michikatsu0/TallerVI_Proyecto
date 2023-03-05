using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformsProvider
{
    void PositionPlatform(PlatformMoveTypes platformType);
    void RotationPlatform();
    void CircularPlatform();
}
