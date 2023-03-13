using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logic : MonoBehaviour
{
    public PlayerControl move;

    private void OnTriggerStay(Collider other)
    {
        move.isGrounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        move.isGrounded = false;
    }
}

