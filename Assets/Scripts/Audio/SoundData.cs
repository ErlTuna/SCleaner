using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Sound Data", menuName = "ScriptableObjects/Audio/Sound Data")]
public class SoundData : ScriptableObject
{
    public AudioClip clip;
    public bool loop;
    public bool playOnAwake;
    public float pitch;
    public bool withVaryingPitch;
    public float pitchRange = 0.1f;
}
