using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class WeaponSoundManager : MonoBehaviour
{
    [SerializeField] GameObject _soundEmitterPrefab;
    [SerializeField] int _maxEmitters = 10;
    readonly List<SoundEmitter> emitters = new();

    void Start()
    {
        // Optionally instantiate one to start
        //CreateNewEmitter();
    }

    void CreateNewEmitter()
    {
        SoundEmitter newEmitter = Instantiate(_soundEmitterPrefab, transform).GetComponent<SoundEmitter>();
        newEmitter.Initialize();
        emitters.Add(newEmitter);
    }

    /*
    public void TryPlayFiringSFX()
    {
        SoundEmitter emitter = null;
        foreach (SoundEmitter e in emitters)
        {
            if (!e.IsPlaying)
            {
                emitter = e;
                break;
            }
        }

        if (emitter == null && emitters.Count < _maxEmitters)
        {
            CreateNewEmitter();
            emitter = emitters[^1];
        }

        if (emitter == null)
            return;

        AudioClip clip = fireSounds[Random.Range(0, fireSounds.Length)];
        float pitch = 1f + Random.Range(-_pitchVariance, _pitchVariance);
        emitter.Play(clip, pitch);
    }
    */

    public void TryPlaySound(SoundData soundData)
    {
        SoundEmitter emitter = null;
        foreach (SoundEmitter e in emitters)
        {
            if (!e.IsPlaying)
            {
                emitter = e;
                break;
            }
        }

        if (emitter == null && emitters.Count < _maxEmitters)
        {
            CreateNewEmitter();
            emitter = emitters[^1];
        }

        if (emitter == null)
            return;


        if (soundData.withVaryingPitch)
        {
            float pitchVariance = soundData.pitchRange;
            float pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
            emitter.Play(soundData.clip, pitch);
        }
        else
            emitter.Play(soundData.clip);
            
    }
}

