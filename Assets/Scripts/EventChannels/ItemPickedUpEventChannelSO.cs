using System;
using UnityEngine;

// Could be split in the future to ItemAddedToInventory and ItemPickedUp (if I ever add a shop system, that is...)
[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Inventory Channels/Item Picked Up Event Channel")]
public class ItemPickedUpEventChannelSO : ScriptableObject
{
    public Action<InventoryItemDefinitionSO> OnEventRaised;
    public void RaiseEvent(InventoryItemDefinitionSO itemData)
    {
        if (OnEventRaised != null)
        {
            Debug.Log("Event raised for weapon added channel");
            OnEventRaised.Invoke(itemData);
        }
    }
}
