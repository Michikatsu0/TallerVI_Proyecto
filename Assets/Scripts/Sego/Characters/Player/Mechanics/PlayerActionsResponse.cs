using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerActionsResponse
{
    public static Action<bool> ActionWeaponDeath;
    public static Action<bool> ActionDashBarCoolDown;
    public static Action<bool> ActionShootWeaponTrigger;
}
