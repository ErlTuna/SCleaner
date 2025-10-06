using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    AudioSource _audioSource;
    Coroutine disableRoutine;
    public bool IsPlaying => _audioSource.isPlaying;

    public void Initialize()
    {
        if (TryGetComponent(out AudioSource audioSource))
            _audioSource = audioSource;
        else
        {
            Debug.Log("AudioSource missing!");
            return;
        }
           

        audioSource.playOnAwake = false;
        gameObject.SetActive(false);
    }

    public void Play(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        if (clip == null) return;

        gameObject.SetActive(true);
        _audioSource.clip = clip;
        _audioSource.pitch = pitch;
        _audioSource.volume = volume;
        _audioSource.Play();

        if (disableRoutine != null)
            StopCoroutine(disableRoutine);

        disableRoutine = StartCoroutine(DisableAfterPlayback());
    }

    private IEnumerator DisableAfterPlayback()
    {
        yield return new WaitWhile(() => _audioSource.isPlaying);
        gameObject.SetActive(false);
    }
}