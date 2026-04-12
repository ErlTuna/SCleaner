using System;
using UnityEngine;

/// <summary>
/// Wrapper class for Unity scenes.
/// </summary>
[Serializable]
public class SceneReference : IEquatable<SceneReference>
{
    [SerializeField] string _sceneName;
    [SerializeField] SceneType _sceneType;

    public string SceneName => _sceneName;
    public SceneType SceneType => _sceneType;

    public bool Equals(SceneReference other)
    {
        return other != null &&
               SceneName == other.SceneName &&
               SceneType == other.SceneType;
    }

    public override bool Equals(object obj) => Equals(obj as SceneReference);
    public override int GetHashCode() => HashCode.Combine(SceneName, SceneType);
}


/// <summary>
/// Type of scene determines whether it should persist across transitions.
/// </summary>
public enum SceneType
{
    PERSISTENT,
    BOOTSTRAP,
    MAIN_MENU,
    PAUSE_MENU,
    LOADING_SCREEN,
    GAMEPLAY_LEVEL,
    UI,
    TEST

}
