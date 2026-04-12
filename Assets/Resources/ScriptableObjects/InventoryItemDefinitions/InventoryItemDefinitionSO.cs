using System;
using UnityEngine;


// Holds the static data for inventory representation of items.

[CreateAssetMenu(fileName = "ItemInventoryRepresentation", menuName = "ScriptableObjects/Inventory Item/Item Inventory Definition")]
public class InventoryItemDefinitionSO : ScriptableObject
{   

    [SerializeField] ItemSO _sourceItem;
    public ItemSO SourceItem => _sourceItem;
    public string ItemID => SourceItem.ItemID;

    public string ItemName;
    [TextArea] public string ItemDescription;
    [TextArea] public string ItemDescriptionShort;
    public Sprite ItemIcon;
    public InventoryItemType ItemType;
    public string ItemTypeDisplay => ItemType.ToDisplayString();
    
}


[Serializable]
public enum InventoryItemType
{
    WEAPON,
    PASSIVE_ITEM
}