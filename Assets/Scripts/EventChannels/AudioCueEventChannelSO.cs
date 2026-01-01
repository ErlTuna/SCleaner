using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Audio Cue Event Channel")]
public class AudioCueEventChannelSO : ScriptableObject
{
    public Action<SoundData, Vector3> OnAudioCueRequested;

    public void RaiseEvent(SoundData soundData, Vector3 location)
    {
        OnAudioCueRequested?.Invoke(soundData, location);
    }
}
