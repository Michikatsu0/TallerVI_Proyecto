using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScifiDoorAudioEvent : MonoBehaviour
{
    private List<AudioClip> audioClips = new List<AudioClip>();
    private AudioSource audioSource;
    private void Start()
    {
        SciFiDoor.LockAnimationEvent += LoadAudioClips;
        audioSource = GetComponentInParent<AudioSource>();
    }

    public void LoadAudioClips(List<AudioClip> audioClips)
    {
        this.audioClips = audioClips;
    }

    private void LockAnimation(int eventNumber)
    {
        if (eventNumber == 0)
            audioSource.PlayOneShot(audioClips[6], 0.5f);
        else
            audioSource.PlayOneShot(audioClips[5], 1f);
    }

    private void DoorAudioFuntion(int eventNumber)
    {
        if (eventNumber == 0)
            audioSource.PlayOneShot(audioClips[0], 0.5f);
        else
            audioSource.PlayOneShot(audioClips[1], 0.5f);
    }
}
