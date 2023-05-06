using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class InteractableProbResponse : MonoBehaviour
{
    public int id;
    [HideInInspector] public bool canInteract = true;

    public void OnTriggerStay(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player"))
        {
            if (canInteract)
            {
                ProbsActionResponse.InteractableUI(true, id);
            }
            else
            {
                ProbsActionResponse.InteractableUI(false, id);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player"))
        {
            if (canInteract)
            {
                ProbsActionResponse.InteractableUI(false, id);
            }
            else
            {
                ProbsActionResponse.InteractableUI(false, id);
            }
        }
    }
}
