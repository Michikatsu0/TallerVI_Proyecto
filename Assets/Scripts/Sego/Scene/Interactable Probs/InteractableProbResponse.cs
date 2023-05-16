using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class InteractableProbResponse : MonoBehaviour
{
    [HideInInspector] public bool canInteract = true;
    [SerializeField] private GameObject interactableButton;

    private void Start()
    {
        interactableButton.SetActive(false);
    }

    public void OnTriggerStay(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player"))
        {
            if (canInteract)            
                interactableButton.SetActive(true);            
            else            
                interactableButton.SetActive(false);
            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player"))
        {
            if (canInteract)
                interactableButton.SetActive(false);
            else
                interactableButton.SetActive(false);
            
        }
    }
}
