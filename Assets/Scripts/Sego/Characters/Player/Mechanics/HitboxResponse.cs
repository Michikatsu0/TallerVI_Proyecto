using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxResponse : MonoBehaviour
{
    public HealthResponse healthResponse;

    public void OnRaycastHit()
    {
        //healthResponse.TakeDamage();
    }
}
