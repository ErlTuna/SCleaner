using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Item Picked Up Event Channel")]
public class ItemPickedUpNotificationEventChannelSO : ScriptableObject
{
    public event Action<PickedUpItemData> OnEventRaised;

    public void RaiseEvent(PickedUpItemData itemData)
    {
        OnEventRaised?.Invoke(itemData);
    }

}

public struct PickedUpItemData
{
    public string name;
    public string description;
    public Sprite icon;
}
