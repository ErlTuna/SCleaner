using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(InventoryItemDefinitionSO))]
public class InventoryItemDefinitionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var so = (InventoryItemDefinitionSO)target;

        EditorGUILayout.Space();

        if (GUILayout.Button("Regenerate Item ID"))
        {
            Undo.RecordObject(so, "Regenerate Item ID");
            typeof(InventoryItemDefinitionSO)
                .GetField("_itemId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(so, System.Guid.NewGuid().ToString());

            EditorUtility.SetDirty(so);
            AssetDatabase.SaveAssets();
        }
    }
}

#endif

