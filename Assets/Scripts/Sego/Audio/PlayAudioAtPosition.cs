using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayAudioAtPosition
{
    public static void PlayClipAtPoint(AudioClip clip, Vector3 position, float volume)
    {
        GameObject gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        AudioSource audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 0.5f;
        audioSource.volume = volume;
        audioSource.Play();
        Object.Destroy((Object)gameObject, clip.length * ((double)Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
    }
}
