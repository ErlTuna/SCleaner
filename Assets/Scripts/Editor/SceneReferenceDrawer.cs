#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomPropertyDrawer(typeof(SceneReference))]
public class SceneReferenceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Properties
        var sceneNameProp = property.FindPropertyRelative("SceneName");
        var sceneTypeProp = property.FindPropertyRelative("SceneType");

        // Layout
        Rect dropdownRect = new Rect(position.x, position.y, position.width * 0.60f, position.height);
        Rect typeRect     = new Rect(position.x + position.width * 0.62f, position.y, position.width * 0.38f, position.height);
        Rect labelRect    = new Rect(dropdownRect.xMax + 4, position.y, 80, position.height);

        // Build list of scenes
        var buildScenes = EditorBuildSettings.scenes;
        string[] sceneNames = new string[buildScenes.Length + 1];

        sceneNames[0] = "<None>";

        for (int i = 0; i < buildScenes.Length; i++)
        {
            sceneNames[i + 1] = Path.GetFileNameWithoutExtension(buildScenes[i].path);
        }

        // Find current index
        int currentIndex = 0;
        for (int i = 1; i < sceneNames.Length; i++)
        {
            if (sceneNames[i] == sceneNameProp.stringValue)
            {
                currentIndex = i;
                break;
            }
        }

        //-------------------------------------------------------------
        // VALIDATION FLAGS
        //-------------------------------------------------------------
        bool isMissing = currentIndex == 0;
        bool isDuplicate = IsSceneNameDuplicate(property);

        //-------------------------------------------------------------
        // DRAW BACKGROUND TINT
        //-------------------------------------------------------------
        if (isMissing)
            DrawTint(dropdownRect, new Color(1f, 0.3f, 0.3f, 0.25f)); // red
        else if (isDuplicate)
            DrawTint(dropdownRect, new Color(1f, 0.85f, 0.3f, 0.25f)); // yellow

        //-------------------------------------------------------------
        // DRAW POPUP
        //-------------------------------------------------------------
        int newIndex = EditorGUI.Popup(dropdownRect, currentIndex, sceneNames);
        if (newIndex != currentIndex)
        {
            sceneNameProp.stringValue = (newIndex == 0 ? "" : sceneNames[newIndex]);
        }

        //-------------------------------------------------------------
        // DRAW STATUS LABEL (does NOT cover popup text)
        //-------------------------------------------------------------
        if (isMissing)
            EditorGUI.LabelField(labelRect, "<Missing>", EditorStyles.boldLabel);
        else if (isDuplicate)
            EditorGUI.LabelField(labelRect, "Duplicate", EditorStyles.boldLabel);

        //-------------------------------------------------------------
        // Draw SceneType enum
        //-------------------------------------------------------------
        EditorGUI.PropertyField(typeRect, sceneTypeProp, GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    //-------------------------------------------------------------
    // Helper: Draw a semi-transparent tint box
    //-------------------------------------------------------------
    private void DrawTint(Rect rect, Color color)
    {
        EditorGUI.DrawRect(rect, color);
    }

    //-------------------------------------------------------------
    // Helper: Check if this SceneName is duplicated in the same list
    //-------------------------------------------------------------
    private bool IsSceneNameDuplicate(SerializedProperty property)
    {
        // Find parent array
        SerializedProperty parent = property.serializedObject.FindProperty("scenes");

        if (parent == null || !parent.isArray)
            return false;

        string targetName = property.FindPropertyRelative("SceneName").stringValue;

        if (string.IsNullOrEmpty(targetName))
            return false;

        int count = 0;

        for (int i = 0; i < parent.arraySize; i++)
        {
            SerializedProperty element = parent.GetArrayElementAtIndex(i);
            string name = element.FindPropertyRelative("SceneName").stringValue;

            if (name == targetName)
                count++;
        }

        return count > 1;
    }
}
#endif
