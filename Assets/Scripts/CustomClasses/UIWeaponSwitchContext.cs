using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

// Used for UI updates when weapons are switched.
public readonly struct UIWeaponSwitchContext
{
    public readonly Sprite weaponIcon;
    public readonly int currentAmmo;
    public readonly int reserveAmmo;
    public readonly bool hasInfiniteReserveAmmo;

    public UIWeaponSwitchContext(Sprite icon, int current, int reserve, bool hasInfiniteReserve)
    {
        weaponIcon = icon;
        currentAmmo = current;
        reserveAmmo = reserve;
        hasInfiniteReserveAmmo = hasInfiniteReserve;
    }
}

