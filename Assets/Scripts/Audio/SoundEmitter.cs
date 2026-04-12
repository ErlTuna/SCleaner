using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{   
    [SerializeField] Transform _poolParent;
    Action<SoundEmitter> _returnToPoolCallback;
    AudioSource _audioSource;
    Coroutine disableRoutine;
    public bool IsPlaying => _audioSource.isPlaying;

    public void Initialize()
    {

    }

    void Awake()
    {
        if (TryGetComponent(out AudioSource audioSource))
            _audioSource = audioSource;
        else
        {
            Debug.Log("AudioSource missing!");
            return;
        }

        _audioSource.playOnAwake = false;
        //if (_returnToPoolCallback != null)
            //gameObject.SetActive(false);
    }

    

    public void Play(AudioClip clip, float pitch = 1f, float volume = 1f)
    {
        if (clip == null) return;

        gameObject.SetActive(true);
        _audioSource.clip = clip;
        _audioSource.pitch = pitch;
        //_audioSource.volume = SETTINGS.CurrentSFXVolume;
        _audioSource.volume = volume;
        _audioSource.Play();

        if (disableRoutine != null)
            StopCoroutine(disableRoutine);

        disableRoutine = StartCoroutine(DisableAfterPlayback());
    }

    public void PlayAtLocation(AudioClip clip, Vector3 location, float pitch = 1f, float volume = 1f)
    {
        if (clip == null) return;
        
        gameObject.transform.parent = null;
        gameObject.transform.position = location;
        gameObject.SetActive(true);
        _audioSource.clip = clip;
        _audioSource.pitch = pitch;
        //_audioSource.volume = SETTINGS.CurrentSFXVolume;
        _audioSource.volume = volume;
        _audioSource.Play();

        if (disableRoutine != null)
            StopCoroutine(disableRoutine);

        disableRoutine = StartCoroutine(DisableAfterPlayback());
    }

    IEnumerator DisableAfterPlayback()
    {
        //float clipLength = _audioSource.clip != null ? _audioSource.clip.length / _audioSource.pitch : 0f;
        yield return new WaitWhile(() => _audioSource.isPlaying);
        //yield return new WaitForSeconds(clipLength);
        if (_returnToPoolCallback != null)
            _returnToPoolCallback.Invoke(this);

        else
            gameObject.SetActive(false);

        
        transform.SetParent(_poolParent);
        transform.localPosition = Vector3.zero;
    }

    public void SetReturnAction(Action<SoundEmitter> onReturn)
    {
        _returnToPoolCallback = onReturn;
    }

    public void AssignPoolParent(Transform parentTransform)
    {
        _poolParent = parentTransform;
    }
}