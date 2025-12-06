using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public readonly struct WeaponUpdateData
{
    public readonly Sprite weaponIcon;
    public readonly int currentAmmo;
    public readonly int reserveAmmo;

    public WeaponUpdateData(Sprite icon, int current, int reserve)
    {
        weaponIcon = icon;
        currentAmmo = current;
        reserveAmmo = reserve;
    }
}

