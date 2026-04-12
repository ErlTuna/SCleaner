using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Audio/Scene Music Library")]
public class SceneMusicLibrarySO : ScriptableObject
{
    public List<LevelMusic> Entries;
    private Dictionary<SceneReference, AudioClip> _lookup;

    public void Initialize()
    {
        _lookup = new Dictionary<SceneReference, AudioClip>();

        foreach (var entry in Entries)
        {
            if (entry.Scene == null)
                continue;

            _lookup[entry.Scene] = entry.LevelBGM;
        }
    }

    public AudioClip GetMusic(SceneReference scene)
    {
        if (_lookup == null)
            Initialize();

        return _lookup.TryGetValue(scene, out var clip) ? clip : null;
    }
}
