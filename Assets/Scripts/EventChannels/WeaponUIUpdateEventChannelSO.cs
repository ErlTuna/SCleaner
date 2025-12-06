using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Weapon Switch UI Update Event Channel")]
public class WeaponUIUpdateEventChannelSO : ScriptableObject
{
    public Action<WeaponUpdateData> OnEventRaised;

    public void RaiseEvent(WeaponUpdateData weaponUpdateData)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(weaponUpdateData);
    }
}
