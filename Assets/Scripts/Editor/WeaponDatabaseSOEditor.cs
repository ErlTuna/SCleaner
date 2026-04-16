#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CustomEditor(typeof(WeaponDatabaseSO))]
public class WeaponDatabaseSOEditor : Editor
{
    private Vector2 scroll;

    private const float ID_WIDTH = 250f;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var db = (WeaponDatabaseSO)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Weapon Database", EditorStyles.boldLabel);

        if (db.Weapons == null)
        {
            EditorGUILayout.HelpBox("Weapon list is null.", MessageType.Warning);
            return;
        }

        DrawTableHeader();

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(300));

        foreach (var weapon in db.Weapons)
        {
            DrawRow(weapon);
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawTableHeader()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

        GUILayout.Label("ID", CenteredHeaderStyle(), GUILayout.Width(ID_WIDTH));
        DrawSeparator();
        GUILayout.Label("NAME", CenteredHeaderStyle(), GUILayout.ExpandWidth(true));

        EditorGUILayout.EndHorizontal();
    }

    private void DrawRow(PlayerWeaponConfigSO weapon)
    {
        EditorGUILayout.BeginHorizontal();

        string id = weapon != null ? weapon.InventoryItemGUID : "NULL";
        string name = weapon != null ? weapon.name : "Missing Reference";

        GUILayout.Label(id, GUILayout.Width(ID_WIDTH));
        DrawSeparator();
        GUILayout.Label(name, GUILayout.ExpandWidth(true));

        EditorGUILayout.EndHorizontal();
    }

    private void DrawSeparator()
    {
        Rect r = GUILayoutUtility.GetRect(1, 18, GUILayout.Width(1));
        EditorGUI.DrawRect(r, new Color(0.3f, 0.3f, 0.3f, 1f));
    }

    private GUIStyle CenteredHeaderStyle()
    {
        return new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter
        };
    }
}