using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Audio Cue Event Channel")]
public class AudioCueEventChannelSO : ScriptableObject
{
    public Action<SoundDataSO, Vector3> OnAudioCueRequested;

    public void RaiseEvent(SoundDataSO soundData, Vector3 location)
    {
        OnAudioCueRequested?.Invoke(soundData, location);
    }
}
