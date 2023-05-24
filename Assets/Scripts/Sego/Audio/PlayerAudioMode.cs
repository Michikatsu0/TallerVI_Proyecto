using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioMode : MonoBehaviour
{
    public static PlayerAudioMode Instance;
    public static Action<bool> OnCombat;
    [SerializeField] private float volume, lerpFadeVolume;
    [SerializeField] private AudioUISettings audioSettings;
    private AudioSource audioSource;
    Animator animator;
    private bool onCombat, flagClip = true;
    void Start()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        audioSource.spatialBlend = 0.5f;
        audioSource.loop = true;
        audioSource.playOnAwake = true; 
        OnCombat += MixerSong;
    }

    void Update()
    {
        if (onCombat && flagClip)
        {
            if (flagClip)
            {
                var audioClip = audioSettings.gameCombatClips[UnityEngine.Random.Range(0, audioSettings.gameCombatClips.Count)];
                flagClip = false;
            }
        }
        else
        {
            if (flagClip)
            {
                var audioClip = audioSettings.gameCombatClips[UnityEngine.Random.Range(0, audioSettings.gameCombatClips.Count)];
                flagClip = false;
            }
        }
    }

    public void MixerSong(bool onCombat)
    {
        this.onCombat = onCombat;
    }
}
