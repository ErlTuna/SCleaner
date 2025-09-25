using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class WeaponSoundManager : MonoBehaviour
{
    AudioSource _audioSource;

    void Start(){
        _audioSource = GetComponent<AudioSource>();
    }

    void PlayWeaponSFX(AudioClip sfx){
        if(_audioSource != null & sfx != null)
        _audioSource.PlayOneShot(sfx);
    }


}
