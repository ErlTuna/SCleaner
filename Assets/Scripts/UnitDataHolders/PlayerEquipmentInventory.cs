using System;
using UnityEngine;

[Serializable]
public class PlayerEquipmentInventory
{
    readonly ItemPickedUpEventChannelSO _equipmentPickedUpEventChannel;
    readonly ItemDroppedEventChannel _equipmentRemovedEventChannel;
    public GameObject EquipmentGO;
    public BaseEquipment EquipmentScript;

    public PlayerEquipmentInventory(
                                ItemPickedUpEventChannelSO EquipmentPickedUpInventoryUpdateChannel, 
                                ItemDroppedEventChannel EquipmentDroppedUpInventoryUpdateChannel
                              )
    {
        _equipmentPickedUpEventChannel = EquipmentPickedUpInventoryUpdateChannel;
        _equipmentRemovedEventChannel = EquipmentDroppedUpInventoryUpdateChannel;
    }

    public void AddEquipment(GameObject equipmentGO, BaseEquipment equipmentScript)
    {
        EquipmentGO = equipmentGO;
        EquipmentScript = equipmentScript;
    }

    public void AddEquipmentToInventory(GameObject weapon, PlayerWeapon weaponScript)
    {
        if (_equipmentPickedUpEventChannel != null)
        {
            // Pass the inventory item SO to UI
            //_equipmentPickedUpEventChannel.RaiseEvent(weaponScript.WeaponConfig.InventoryItem);
        }
    }

    // Inventory manager does the necessary checks
    public void RemoveEquipmentFromInventory(GameObject weapon, PlayerWeapon weaponScript)
    {
        if (_equipmentRemovedEventChannel != null)
        {
            // Pass item ID to inventory UI
            //_equipmentRemovedEventChannel.RaiseEvent(weaponScript.WeaponConfig.InventoryItemID);
        }

    }
}
