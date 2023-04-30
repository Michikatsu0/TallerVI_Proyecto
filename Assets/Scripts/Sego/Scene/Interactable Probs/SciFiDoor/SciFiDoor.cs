using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
public class SciFiDoor : MonoBehaviour
{
    enum ScreenStates { open, close, waiting}

    [SerializeField] private bool locked = true, interactableButton, isBroken;
    [SerializeField] private List<Color> doorColors;
    [SerializeField] private List<Color> screenColors;
    [SerializeField] private List<Material> materials = new List<Material>();
    [SerializeField] private List<Texture> screenTextures = new List<Texture>();

    private float delayLockedColor = 0.5f;
    private bool justLocked = true, justLockedColor = true;
    private Animator animator;
    private InteractableProbResponse interactableProb;
    private TextureBaseMaterialScrollOffset textureScrollOffset;
    
    void Start()
    {
        ProbsActionResponse.InteractableButtonUI += InteractuableButton;
        animator = GetComponentInChildren<Animator>();
        interactableProb = GetComponentInChildren<InteractableProbResponse>();
        textureScrollOffset = GetComponent<TextureBaseMaterialScrollOffset>();
        materials[1].SetTexture("_EmissionMap", screenTextures[0]);
        materials[1].SetColor("_EmissionColor", Color.white);
        textureScrollOffset.offSetX = 0.1f;
        textureScrollOffset.offSetY = 0.1f;
    }
    private void Update()
    {
        if (locked)
        {
            if (justLocked)
            {
                justLocked = false;
                materials[0].SetColor("_EmissionColor", doorColors[2]);
                materials[2].SetColor("_EmissionColor", screenColors[1]);
            }
            interactableProb.canInteract = true;
        }
        else
        {
            StopCoroutine(LockedDoorColor());
            if (!justLocked)
            {
                justLocked = true;
                materials[0].SetColor("_EmissionColor", doorColors[1]);
                materials[2].SetColor("_EmissionColor", doorColors[1]);
            }
            interactableProb.canInteract = false;
        }

        if (interactableButton)
        {
            interactableProb.canInteract = false;
        }
    }

    public void InteractuableButton(bool interactableButton, int id)
    {
        if (interactableProb.id == id)
        {
            this.interactableButton = interactableButton;
            locked = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player"))
        {
            if (!locked)
            {
                materials[0].SetColor("_EmissionColor", doorColors[0]);
                materials[1].SetTexture("_EmissionMap", screenTextures[1]);
                materials[1].SetColor("_EmissionColor", screenColors[0]);
                materials[2].SetColor("_EmissionColor", screenColors[0]);
                textureScrollOffset.offSetX = 0;
                textureScrollOffset.offSetY = 0.1f;

                if (!isBroken)
                    animator.SetBool("IsOpen", true);
                else
                    animator.SetBool("IsOpenBroken", true);
            }
            else
            {
                if (justLockedColor)
                {
                    justLockedColor = false;
                    StartCoroutine(LockedDoorColor());
                }
                if (!isBroken)
                    animator.SetBool("IsOpen", false);
                else
                    animator.SetBool("IsOpenBroken", false);
            }
        }
    }

    private IEnumerator LockedDoorColor()
    {
        materials[1].SetTexture("_EmissionMap", screenTextures[1]);
        materials[1].SetColor("_EmissionColor", screenColors[1]);
        textureScrollOffset.offSetX = 0;
        textureScrollOffset.offSetY = 0.1f;

        materials[0].SetColor("_EmissionColor", doorColors[1]);
        materials[2].SetColor("_EmissionColor", doorColors[1]);
        yield return new WaitForSeconds(delayLockedColor);
        materials[0].SetColor("_EmissionColor", doorColors[2]);
        materials[2].SetColor("_EmissionColor", doorColors[2]);
        yield return new WaitForSeconds(delayLockedColor);
        materials[0].SetColor("_EmissionColor", doorColors[1]);
        materials[2].SetColor("_EmissionColor", doorColors[1]);
        yield return new WaitForSeconds(delayLockedColor);
        materials[0].SetColor("_EmissionColor", doorColors[2]);
        materials[2].SetColor("_EmissionColor", doorColors[2]);
        yield return new WaitForSeconds(delayLockedColor);
        materials[0].SetColor("_EmissionColor", doorColors[1]);
        materials[2].SetColor("_EmissionColor", doorColors[1]);
        yield return new WaitForSeconds(delayLockedColor);
        materials[0].SetColor("_EmissionColor", doorColors[2]);
        materials[2].SetColor("_EmissionColor", doorColors[2]);

        materials[1].SetTexture("_EmissionMap", screenTextures[0]);
        materials[1].SetColor("_EmissionColor", Color.white);
        textureScrollOffset.offSetX = 0.1f;
        textureScrollOffset.offSetY = 0.1f;
        yield return null;
    }

    public void OnTriggerExit(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player"))
        {
            if (!justLockedColor)
            {
                justLockedColor = true;
            }

            if (!locked)
            {
                materials[0].SetColor("_EmissionColor", doorColors[1]);
                materials[1].SetTexture("_EmissionMap", screenTextures[0]);
                materials[1].SetColor("_EmissionColor", Color.white);
                materials[2].SetColor("_EmissionColor", screenColors[0]);
                textureScrollOffset.offSetX = 0.1f;
                textureScrollOffset.offSetY = 0.1f;

                if (!isBroken)
                    animator.SetBool("IsOpen", false);
                else
                    animator.SetBool("IsOpenBroken", false);
            }
            else
            {
                if (!isBroken)
                    animator.SetBool("IsOpen", false);
                else
                    animator.SetBool("IsOpenBroken", false);
            }
        }
    }
      
    private void OnDestroy()
    {
        ProbsActionResponse.InteractableButtonUI -= InteractuableButton;
    }
}
