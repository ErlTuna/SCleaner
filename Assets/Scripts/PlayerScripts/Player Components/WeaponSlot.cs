using System;
using UnityEngine;

/// <summary>
/// Wrapper class used by PlayerInventoryManager.
/// Caches the essential data that defines a weapon:
/// its GameObject and the associated BaseWeapon script.
/// </summary>
/// 
[Serializable]
public class WeaponSlot
{
    public WeaponInstance WeaponInstance {get; private set;}

    public WeaponSlot(WeaponInstance instance)
    {
        WeaponInstance = instance;
    }
    public void ClearSlot()
    {
        WeaponInstance = null;
    }


}
