using System;
using UnityEngine;

// Need to change this...

[CreateAssetMenu(menuName = "ScriptableObjects/Event Channels/Weapon Switch Request Event Channel")]
public class WeaponSwitchRequestEventChannelSO : ScriptableObject
{
    public Action<GameObject, PlayerWeapon> OnEventRaised;

    public void RaiseEvent(GameObject targetWeapon, PlayerWeapon targetWeaponScript)
    {
        OnEventRaised?.Invoke(targetWeapon, targetWeaponScript);
    }
}

