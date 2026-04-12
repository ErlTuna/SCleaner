#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

[CustomPropertyDrawer(typeof(SceneReference))]
public class SceneReferenceDrawer : PropertyDrawer
{
    private static string[] cachedSceneNames;

    //-------------------------------------------------------------
    // Static initialization (called on domain reload)
    //-------------------------------------------------------------
    [InitializeOnLoadMethod]
    private static void Init()
    {
        CacheScenes();
        EditorBuildSettings.sceneListChanged += CacheScenes;
    }

    //-------------------------------------------------------------
    // Cache Build Settings scenes
    //-------------------------------------------------------------
    private static void CacheScenes()
    {
        var buildScenes = EditorBuildSettings.scenes;

        cachedSceneNames = new string[buildScenes.Length + 1];
        cachedSceneNames[0] = "<None>";

        for (int i = 0; i < buildScenes.Length; i++)
        {
            cachedSceneNames[i + 1] =
                Path.GetFileNameWithoutExtension(buildScenes[i].path);
        }
    }

    //-------------------------------------------------------------
    // Main GUI
    //-------------------------------------------------------------
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (cachedSceneNames == null)
            CacheScenes();

        EditorGUI.BeginProperty(position, label, property);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        position = EditorGUI.PrefixLabel(position,
            GUIUtility.GetControlID(FocusType.Passive), label);

        //---------------------------------------------------------
        // Get properties
        //---------------------------------------------------------
        var sceneNameProp = property.FindPropertyRelative("_sceneName");
        var sceneTypeProp = property.FindPropertyRelative("_sceneType");

        //---------------------------------------------------------
        // Layout
        //---------------------------------------------------------
        float spacing = 4f;
        float dropdownWidth = position.width * 0.6f;
        float typeWidth = position.width - dropdownWidth - spacing;

        Rect dropdownRect = new Rect(position.x, position.y, dropdownWidth, position.height);
        Rect typeRect = new Rect(position.x + dropdownWidth + spacing, position.y, typeWidth, position.height);

        //---------------------------------------------------------
        // Find current index
        //---------------------------------------------------------
        int currentIndex = 0;
        for (int i = 1; i < cachedSceneNames.Length; i++)
        {
            if (cachedSceneNames[i] == sceneNameProp.stringValue)
            {
                currentIndex = i;
                break;
            }
        }

        //---------------------------------------------------------
        // Validation
        //---------------------------------------------------------
        bool isMissing = currentIndex == 0;
        bool isDuplicate = IsDuplicate(property, sceneNameProp.stringValue);

        //---------------------------------------------------------
        // Tint background
        //---------------------------------------------------------
        if (isMissing)
            DrawTint(dropdownRect, new Color(1f, 0.3f, 0.3f, 0.25f));
        else if (isDuplicate)
            DrawTint(dropdownRect, new Color(1f, 0.85f, 0.3f, 0.25f));

        //---------------------------------------------------------
        // Scene dropdown
        //---------------------------------------------------------
        int newIndex = EditorGUI.Popup(dropdownRect, currentIndex, cachedSceneNames);

        if (newIndex != currentIndex)
        {
            string newName = (newIndex == 0) ? "" : cachedSceneNames[newIndex];
            sceneNameProp.stringValue = newName;

            // Optional: auto-assign SceneType
            /*
            if (!string.IsNullOrEmpty(newName))
            {
                var inferred = InferSceneType(newName);
                sceneTypeProp.enumValueIndex = (int)inferred;
            }
            */
        }

        //---------------------------------------------------------
        // SceneType (locked or editable)
        //---------------------------------------------------------
        //EditorGUI.BeginDisabledGroup(true); // lock editing
        EditorGUI.PropertyField(typeRect, sceneTypeProp, GUIContent.none);
        EditorGUI.EndDisabledGroup();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    //-------------------------------------------------------------
    // Duplicate check (safe, not property-path dependent)
    //-------------------------------------------------------------
    private bool IsDuplicate(SerializedProperty property, string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
            return false;

        var target = property.serializedObject.targetObject;

        if (target is SceneDatabaseSO db)
        {
            int count = db.SceneRefs.Count(s => s.SceneName == sceneName);
            return count > 1;
        }

        return false;
    }

    //-------------------------------------------------------------
    // Scene type inference
    //-------------------------------------------------------------
    SceneType InferSceneType(string sceneName)
    {
        string lower = sceneName.ToLower();

        if (lower.Contains("menu"))
            return SceneType.MAIN_MENU;

        if (lower.Contains("ui"))
            return SceneType.UI;

        if (lower.Contains("bootstrap"))
            return SceneType.BOOTSTRAP;

        if (lower.Contains("loading"))
            return SceneType.LOADING_SCREEN;

        return SceneType.GAMEPLAY_LEVEL;
    }

    //-------------------------------------------------------------
    // Draw tint
    //-------------------------------------------------------------
    private void DrawTint(Rect rect, Color color)
    {
        EditorGUI.DrawRect(rect, color);
    }
}
#endif