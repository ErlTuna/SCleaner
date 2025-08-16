using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPlayerSounds : MonoBehaviour
{
    static AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public static void PlayAudio(AudioClip sound, float volumeScale = 1f){
        if(_audioSource != null & sound != null)
        _audioSource.PlayOneShot(sound, volumeScale);
    }
}
