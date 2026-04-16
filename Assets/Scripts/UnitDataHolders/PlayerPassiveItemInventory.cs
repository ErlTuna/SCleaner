using System.Collections.Generic;
using UnityEngine;

public class PlayerPassiveItemInventory
{
    readonly ItemPickedUpEventChannelSO _passiveItemAddedEventChannel;
    readonly ItemDroppedEventChannel _passiveItemDroppedEventChannel;

    HashSet<PassiveItemSO> _itemSet = new();

    public PlayerPassiveItemInventory(ItemPickedUpEventChannelSO itemPickedUpEventChannel, 
                                      ItemDroppedEventChannel itemDroppedEventChannel)
    {
        _passiveItemAddedEventChannel = itemPickedUpEventChannel;
        _passiveItemDroppedEventChannel = itemDroppedEventChannel;
    }

    public void AddPassiveItemToInventory(PassiveItemSO passiveItem)
    {
        if (passiveItem != null && HasItem(passiveItem) == false)
        {
            Debug.Log("Added item : " + passiveItem);
            _itemSet.Add(passiveItem);

            _passiveItemAddedEventChannel.RaiseEvent(passiveItem.InventoryDefinition);
        }
    }

    public void RemovePassiveItemToInventory(PassiveItemSO passiveItem)
    {
        if (HasItem(passiveItem))
            _itemSet.Remove(passiveItem);

    }

    public bool HasItem(PassiveItemSO passiveItem)
    {
        if (_itemSet.Contains(passiveItem))
            return true;
        
        return false;
    }

}
