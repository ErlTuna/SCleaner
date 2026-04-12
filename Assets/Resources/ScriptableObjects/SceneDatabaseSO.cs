using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObjects/Tables/Scene Database")]
public class SceneDatabaseSO : ScriptableObject
{   
    [Header("Unchanging Scenes")]
    public SceneReference BootstrapSceneRef;
    public SceneReference PersistentSceneRef;
    public SceneReference LoadingScreenRef;
    public SceneReference TestSceneRef;
    public SceneReference MainMenuRef;
    public SceneReference GameplayUIRef;
    public SceneReference PauseMenuRef;
    public SceneReference GameOverUIRef;

    [Header("All Scenes")]
    [Tooltip("This list is used to build non-serialized lists when the SO is enabled.")]
    public List<SceneReference> SceneRefs;

    [NonSerialized] public List<SceneReference> GameplayLevels;
    [NonSerialized] public List<SceneReference> UIScenes;
    [NonSerialized] public SceneReference MainMenu;
    [NonSerialized] public SceneReference PauseMenu;
    [NonSerialized] public SceneReference GameOverUI;

    HashSet<string> sceneNameSet;

    public void BuildTypeLists()
    {
        GameplayLevels = SceneRefs.Where(s => s.SceneType == SceneType.GAMEPLAY_LEVEL).ToList();
        UIScenes        = SceneRefs.Where(s => s.SceneType == SceneType.UI).ToList();
        MainMenu        = SceneRefs.FirstOrDefault(s => s.SceneType == SceneType.MAIN_MENU);
        PauseMenu       = SceneRefs.FirstOrDefault(s => s.SceneType == SceneType.UI && s.SceneName.Contains("Pause"));
        GameOverUI      = SceneRefs.FirstOrDefault(s => s.SceneType == SceneType.UI && s.SceneName.Contains("GameOver"));
    }

    void OnEnable()
    {
        BuildTypeLists();
    }

    void OnValidate()
    {
        #if UNITY_EDITOR
        // Avoid spamming validation while the game is running
        if (Application.isPlaying)
            return;

        // Track duplicate scene names
        sceneNameSet = new HashSet<string>();

        foreach (var s in SceneRefs)
        {
            if (string.IsNullOrWhiteSpace(s.SceneName))
            {
                Debug.LogError($"[SceneDatabase] Scene name cannot be empty.", this);
                continue;
            }

            // Check duplicates
            if (sceneNameSet.Add(s.SceneName) == false)
            {
                Debug.LogError($"[SceneDatabase] Duplicate scene name found: {s.SceneName}", this);
            }

            // Check existence in Build Settings
            if (DoesSceneExistInBuildSettings(s.SceneName) == false)
                Debug.LogWarning($"[SceneDatabase] Scene '{s.SceneName}' does NOT exist in Build Settings.", this);
        }
        #endif
    }

    #if UNITY_EDITOR
    bool DoesSceneExistInBuildSettings(string sceneName)
    {
        foreach (var s in EditorBuildSettings.scenes)
        {
            string fileName = Path.GetFileNameWithoutExtension(s.path);

            if (fileName.Equals(sceneName, System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }   

        return false;
    
    }
    #endif

    public bool ContainsScene(string sceneName)
    {
        if (sceneNameSet == null)
            RebuildHashSet();
        
        return sceneNameSet.Contains(sceneName);
    }

    public bool ContainsScene(SceneReference scene)
    {
        return SceneRefs.Contains(scene);
    }

    void RebuildHashSet()
    {
        sceneNameSet = new HashSet<string>();

        foreach (var sceneRef in SceneRefs)
        {
            if (sceneNameSet.Add(sceneRef.SceneName) != true)
            {
                Debug.LogError($"Duplicate scene name: {sceneRef.SceneName}");
            }
        }
    }

    public SceneType GetSceneType(string sceneName)
    {
        var sceneRef = SceneRefs.Find(s => s.SceneName == sceneName);
        return sceneRef.SceneType;
    }

    public SceneType GetSceneType(SceneReference scene)
    {
        return scene.SceneType;
    }

    public SceneReference GetLevel(int levelIndex)
    {
        if (GameplayLevels == null) return null;
        if (GameplayLevels.Count - 1 < levelIndex) return null;

        Debug.Log("Pulled scene : " + GameplayLevels[levelIndex].SceneName);
        return GameplayLevels[levelIndex];
    }
}

