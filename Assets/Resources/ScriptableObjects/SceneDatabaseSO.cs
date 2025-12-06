using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

[CreateAssetMenu(menuName = "Scene Management/Scene Database")]
public class SceneDatabaseSO : ScriptableObject
{
    public List<SceneReference> scenes;

    HashSet<string> sceneNameSet;

    void OnValidate()
    {
        #if UNITY_EDITOR
        // Avoid spamming validation while the game is running
        if (Application.isPlaying)
            return;

        // Track duplicate scene names
        sceneNameSet = new HashSet<string>();

        foreach (var s in scenes)
        {
            if (string.IsNullOrWhiteSpace(s.SceneName))
            {
                Debug.LogError($"[SceneDatabase] Scene name cannot be empty.", this);
                continue;
            }

            // Check duplicates
            if (!sceneNameSet.Add(s.SceneName))
            {
                Debug.LogError($"[SceneDatabase] Duplicate scene name found: {s.SceneName}", this);
            }

            // Check existence in Build Settings
            if (!DoesSceneExistInBuildSettings(s.SceneName))
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

    void RebuildHashSet()
    {
        sceneNameSet = new HashSet<string>();

        foreach (var sceneRef in scenes)
        {
            if (sceneNameSet.Add(sceneRef.SceneName) != true)
            {
                Debug.LogError($"Duplicate scene name: {sceneRef.SceneName}");
            }
        }
    }

    public SceneType GetSceneType(string sceneName)
    {
        var sceneRef = scenes.Find(s => s.SceneName == sceneName);
        return sceneRef.SceneType;
    }
}

