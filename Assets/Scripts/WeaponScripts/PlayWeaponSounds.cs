using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayWeaponSounds
{
    static AudioSource _source;

    public static void ReceiveAudioSource(AudioSource audioSource){
        _source = audioSource;
    }

    public static void PlayGunSound(AudioClip gunSound){

        _source.clip = gunSound;
        if (!_source.isPlaying){
            _source.Play();
        }
    }
}
