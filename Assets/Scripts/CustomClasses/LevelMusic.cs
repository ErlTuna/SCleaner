using System;
using UnityEngine;

[Serializable]
public class LevelMusic
{
    public SceneReference Scene;
    [SerializeField] AudioClip _levelBGM;
    public AudioClip LevelBGM => _levelBGM;
}
