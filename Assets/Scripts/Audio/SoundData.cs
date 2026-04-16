using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Sound Data", menuName = "ScriptableObjects/Audio/Sound Data")]
public class SoundDataSO : ScriptableObject
{
    public AudioClip[] clips;
    public bool loop;
    public bool playOnAwake;
    public bool playRandomAmongGroup;
    public float pitch;
    public bool withVaryingPitch;
    public float pitchRange = 0.1f;
}
