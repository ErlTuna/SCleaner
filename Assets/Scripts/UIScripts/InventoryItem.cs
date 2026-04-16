using System;

// Wrapper class that stores the inventory representation data of an item.
// This class can also hold runtime data for an item. (will change into an abstract class in that case)
[Serializable]
public class InventoryItem
{
    public InventoryItemDefinitionSO ItemData;
    public InventoryItem(InventoryItemDefinitionSO itemData)
    {
        ItemData = itemData;
    }

}
