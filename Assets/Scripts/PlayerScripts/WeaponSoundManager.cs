using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof (AudioSource))]
public class WeaponSoundManager : MonoBehaviour
{
    AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayWeaponSFX(AudioClip sfx)
    {
        if (_audioSource != null & sfx != null)
            _audioSource.PlayOneShot(sfx);
    }


}
