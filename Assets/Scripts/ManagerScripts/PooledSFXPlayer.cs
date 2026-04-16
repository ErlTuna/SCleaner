using System.Collections.Generic;
using UnityEngine;

public class PooledSFXPlayer : MonoBehaviour
{
    [SerializeField] GameObject _soundEmitterPrefab;
    [SerializeField] int _maxEmitters = 10;
    [SerializeField] float _emitterVolumeLevel = 5f;
    readonly List<SoundEmitter> emitters = new();

    void CreateNewEmitter()
    {
        SoundEmitter newEmitter = Instantiate(_soundEmitterPrefab, transform).GetComponent<SoundEmitter>();
        //newEmitter.Initialize();
        newEmitter.AssignPoolParent(transform);
        emitters.Add(newEmitter);
    }

    public void TryPlaySound(SoundDataSO soundData)
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
        {
            return;
        }

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
            emitter.Play(clipToBePlayed, pitch, _emitterVolumeLevel);
        }

        else
            emitter.Play(clipToBePlayed, pitch: 1, _emitterVolumeLevel);

    }

    public void TryPlaySound(SoundDataSO soundData, Vector3 location)
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
        {
            return;
        }
            


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
            emitter.PlayAtLocation(clipToBePlayed, location, pitch, _emitterVolumeLevel);
        }

        else
            emitter.PlayAtLocation(clipToBePlayed, location, pitch: 1, _emitterVolumeLevel);

    }


    public void TryPlaySoundWithVolume(SoundDataSO soundData, Vector3 location, float volume)
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
        {
            return;
        }
            


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
            emitter.PlayAtLocation(clipToBePlayed, location, pitch, _emitterVolumeLevel);
        }

        else
            emitter.PlayAtLocation(clipToBePlayed, location, pitch: 1, volume);

    }
    
    public AudioClip GetClip(SoundDataSO soundData)
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


    public void SetEmitterVolume(float volume)
    {
        _emitterVolumeLevel = volume;
    }
}
