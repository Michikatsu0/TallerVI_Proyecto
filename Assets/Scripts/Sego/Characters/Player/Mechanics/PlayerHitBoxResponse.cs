using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBoxResponse : MonoBehaviour
{
    public PlayerHealthResponse healthResponse;

    public void OnRaycastHit()
    {
        //healthResponse.TakeDamage();
    }
}
