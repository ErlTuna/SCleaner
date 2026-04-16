using UnityEngine;

public class PlayerPassiveItemInventoryRuntime
{   
    readonly ItemPickedUpEventChannelSO _passiveItemAddedEventChannel;
    readonly ItemDroppedEventChannel _passiveItemDroppedEventChannel;
    PlayerPassiveItemInventoryData _data;

    public PlayerPassiveItemInventoryRuntime(PlayerPassiveItemInventoryData data, PassiveItemInventoryDependencies dependencies)
    {
        _data = data;
        _passiveItemAddedEventChannel = dependencies.PickedUpChannel;
        _passiveItemDroppedEventChannel = dependencies.DroppedChannel;
    }

    public void AddPassiveItemToInventory(PassiveItemSO passiveItem)
    {
        if (passiveItem != null && HasItem(passiveItem) == false)
        {
            Debug.Log("Added item : " + passiveItem);
            _data.ItemIDs.Add(passiveItem.ItemID);
            _passiveItemAddedEventChannel.RaiseEvent(passiveItem.InventoryDefinition);
        }
    }

    public void RemovePassiveItemToInventory(PassiveItemSO passiveItem)
    {
        if (HasItem(passiveItem))
            _data.ItemIDs.Remove(passiveItem.ItemID);

    }

    public bool HasItem(PassiveItemSO passiveItem)
    {
        if (_data.ItemIDs.Contains(passiveItem.ItemID))
            return true;
        
        return false;
    }

}
