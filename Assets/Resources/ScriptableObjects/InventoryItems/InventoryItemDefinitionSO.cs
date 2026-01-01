using System;
using UnityEngine;


// Holds the static data for inventory representation of items.

[CreateAssetMenu(fileName = "ItemInventoryRepresentation", menuName = "ScriptableObjects/Inventory Item/Item Inventory Representation")]
public class InventoryItemDefinitionSO : ScriptableObject
{   
    [SerializeField] string _itemId;
    public string ItemID => _itemId;

    public string ItemName;
    [TextArea] public string ItemDescription;
    public Sprite ItemIcon;
    public InventoryItemType ItemType;

    #if UNITY_EDITOR
    void OnValidate()
    {
        if (string.IsNullOrEmpty(_itemId))
        {
            _itemId = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
    #endif

    #if UNITY_EDITOR
    [ContextMenu("Regenerate Item ID")]
    void RegenerateId()
    {
        _itemId = Guid.NewGuid().ToString();
        UnityEditor.EditorUtility.SetDirty(this);
    }
    #endif

    
}


[Serializable]
public enum InventoryItemType
{
    WEAPON,
    EQUIPMENT
}