using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Inventory Channels/Player Weapon Drop Event Channel")]
public class ItemDroppedEventChannel : ScriptableObject
{
    // Start is called before the first frame update
    public Action<string> OnEventRaised;
    public void RaiseEvent(string itemID)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(itemID);
    }
}
