using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Inventory Channels/Player Weapon Picked Up Event Channel")]
public class ItemPickedUpEventChannel : ScriptableObject
{
    public Action<InventoryItemDefinitionSO> OnEventRaised;
    public void RaiseEvent(InventoryItemDefinitionSO itemData)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(itemData);
    }
}
