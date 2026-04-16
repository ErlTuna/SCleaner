using UnityEditor;
using UnityEngine;

/*
[CustomEditor(typeof(ItemSO), true)]
public class PickupSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemSO pickup = (ItemSO)target;

        if (pickup.InventoryDefinition != null)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Item GUID", pickup.InventoryDefinition != null ? pickup.InventoryDefinition.ItemID : "(No ItemSO assigned)");
            EditorGUI.EndDisabledGroup();
        }
    }
}
*/