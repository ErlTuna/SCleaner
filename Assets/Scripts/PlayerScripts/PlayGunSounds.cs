using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayGunSounds : MonoBehaviour
{
    AudioSource _audioSource;

    void Start(){
        _audioSource = GetComponent<AudioSource>();
    }

    void PlayCurrentWeaponAudio(AudioClip fireSound){
        if(_audioSource != null & fireSound != null)
        _audioSource.PlayOneShot(fireSound);
    }


}
