using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Ammo Change Event Channel")]
public class AmmoChangeEventChannelSO : ScriptableObject
{
    public Action<AmmoData> OnEventRaised;

    public void RaiseEvent(AmmoData ammoData)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(ammoData);
    }
}
