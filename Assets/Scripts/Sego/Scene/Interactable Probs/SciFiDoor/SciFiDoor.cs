using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
public class SciFiDoor : MonoBehaviour
{
    public static Action<List<AudioClip>> LockAnimationEvent;

    [SerializeField] private bool locked = true, interactableButton, isBroken;
    [SerializeField] private List<Color> doorColors;
    [SerializeField] private List<Color> screenColors;
    [SerializeField] private List<Material> materials = new List<Material>();
    [SerializeField] private List<Texture> screenTextures = new List<Texture>();
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();

    private float delayLockedColor = 0.5f;
    private bool justLocked = true, justLockedColor = true;
    private Animator[] animator;
    private InteractableProbResponse interactableProb;
    private TextureBaseMaterialScrollOffset textureScrollOffset;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponentsInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
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
                materials[3].SetColor("_EmissionColor", doorColors[2]);
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
                materials[3].SetColor("_EmissionColor", doorColors[1]);
            }
            interactableProb.canInteract = false;
        }

        if (interactableButton)
        {
            interactableProb.canInteract = false;
        }
    }

    private void ResetInteractableButton()
    {
        interactableButton = false;
    }

    public void InteractuableButton(bool interactableButton)
    {
        LockAnimationEvent?.Invoke(audioClips); //Auxiliar Sound Class
        audioSource.PlayOneShot(audioClips[3], 0.5f);
        StartCoroutine(DelayDoorOpenAudio());
        this.interactableButton = interactableButton;
        locked = false;
        Invoke(nameof(ResetInteractableButton), 0.1f);
    }
    
    private IEnumerator DelayDoorOpenAudio()
    {
        OpenDoorAudio();
        yield return new WaitForSeconds(0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.CompareTag("Player"))
        {
            if (!locked)
            {
                if (!isBroken)
                {
                    OpenDoorAudio();
                }
            }
        }
    }

    private void OpenDoorAudio()
    {
        audioSource.pitch = 1;
        audioSource.volume = 1;
        audioSource.clip = audioClips[4];
        audioSource.PlayDelayed(0.5f);
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
                materials[3].SetColor("_EmissionColor", doorColors[0]);
                textureScrollOffset.offSetX = 0;
                textureScrollOffset.offSetY = 0.1f;

                if (!isBroken)
                    animator[0].SetBool("IsOpen", true);
                else
                    animator[0].SetBool("IsOpenBroken", true);
                
            }
            else
            {
                if (justLockedColor)
                {
                    justLockedColor = false;
                    StartCoroutine(LockedDoorColor());
                    audioSource.clip = audioClips[2];
                    audioSource.Play();
                    audioSource.volume = 0.22f;
                    audioSource.pitch = 0.3f;
                }
                if (!isBroken)
                    animator[0].SetBool("IsOpen", false);
                else
                    animator[0].SetBool("IsOpenBroken", false);
                
            }
        }
    }

    private IEnumerator LockedDoorColor()
    {
        materials[1].SetTexture("_EmissionMap", screenTextures[1]);
        materials[1].SetColor("_EmissionColor", screenColors[1]);

        textureScrollOffset.offSetX = 0;
        textureScrollOffset.offSetY = 0.1f;

        WarningRed();
        yield return new WaitForSeconds(delayLockedColor);
        WarningYellow();
        yield return new WaitForSeconds(delayLockedColor);
        WarningRed();
        yield return new WaitForSeconds(delayLockedColor);
        WarningYellow();
        yield return new WaitForSeconds(delayLockedColor);
        WarningRed();
        yield return new WaitForSeconds(delayLockedColor);
        WarningYellow();

        materials[1].SetTexture("_EmissionMap", screenTextures[0]);
        materials[1].SetColor("_EmissionColor", Color.white);

        textureScrollOffset.offSetX = 0.1f;
        textureScrollOffset.offSetY = 0.1f;

        yield return null;
    }
    private void WarningYellow()
    {
        materials[0].SetColor("_EmissionColor", doorColors[2]);
        materials[2].SetColor("_EmissionColor", doorColors[2]);
        materials[3].SetColor("_EmissionColor", doorColors[2]);
    }
    private void WarningRed()
    {
        materials[0].SetColor("_EmissionColor", doorColors[1]);
        materials[2].SetColor("_EmissionColor", doorColors[1]);
        materials[3].SetColor("_EmissionColor", doorColors[1]);
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
                materials[3].SetColor("_EmissionColor", doorColors[1]);
                textureScrollOffset.offSetX = 0.1f;
                textureScrollOffset.offSetY = 0.1f;

                if (!isBroken)
                {
                    animator[0].SetBool("IsOpen", false);
                }
                else
                    animator[0].SetBool("IsOpenBroken", false);
            }
            else
            {
                if (!isBroken)
                    animator[0].SetBool("IsOpen", false);
                else
                    animator[0].SetBool("IsOpenBroken", false);
            }
        }
    }
}
