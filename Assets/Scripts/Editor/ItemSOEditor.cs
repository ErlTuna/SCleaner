using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemSO), true)]
public class ItemSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var so = (ItemSO)target;

        EditorGUILayout.Space();

        // Show ItemID
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField("Item GUID", so.ItemID);
        EditorGUI.EndDisabledGroup();

                // Button to regenerate
        if (GUILayout.Button("Regenerate Item ID"))
        {
            Undo.RecordObject(so, "Regenerate Item ID");
            so.RegenerateId(); // call the same method
            EditorUtility.SetDirty(so);
            AssetDatabase.SaveAssets();
        }
    }
}

