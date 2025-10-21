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


        AudioClip clipToBePlayed = GetClip(soundData);

        if (clipToBePlayed == null)
        {
            Debug.Log("There isn't a valid clip to play.");
            return;
        }

        if (soundData.withVaryingPitch)
        {
            float pitchVariance = soundData.pitchRange;
            float pitch = 1f + Random.Range(-pitchVariance, pitchVariance);
            emitter.Play(clipToBePlayed, pitch);
        }

        else
            emitter.Play(clipToBePlayed);

    }
    
    public AudioClip GetClip(SoundData soundData)
    {
        if (soundData.clips == null)
        {
            Debug.Log("Passed sound data has no clips.");
            return null;
        }

        if (soundData.playRandomAmongGroup && soundData.clips.Length > 1)
        {
            return soundData.clips[Random.Range(0, soundData.clips.Length)];
        }

        else if (soundData.clips.Length > 0)
        {
            return soundData.clips[0];
        }

        return null;
    }
}

