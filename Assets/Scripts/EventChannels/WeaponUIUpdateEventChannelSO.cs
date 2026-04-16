using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Weapon Switch UI Update Event Channel")]
public class WeaponUIUpdateEventChannelSO : ScriptableObject
{
    public Action<UIWeaponSwitchContext> OnEventRaised;

    public void RaiseEvent(UIWeaponSwitchContext weaponUpdateData)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(weaponUpdateData);
    }
}
