using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInventoryEvents
{
    #region Events
    public static Action<GameObject, BaseWeapon_v2> OnInventoryReadyEvent;
    public static Action<GameObject, BaseWeapon_v2> OnWeaponSwitchEvent;
    public static Action<Sprite, WeaponRuntimeData> OnWeaponSwitchUIUpdate;

    #endregion

    #region Invokers

    public static void RaiseInventoryReadyEvent(GameObject weapon, BaseWeapon_v2 weaponScript)
    {
        OnInventoryReadyEvent?.Invoke(weapon, weaponScript);
        OnWeaponSwitchUIUpdate?.Invoke(weaponScript.WeaponConfig.sprite, weaponScript.WeaponRuntimeData);
    }


    public static void RaiseWeaponSwitchEvent(GameObject weapon, BaseWeapon_v2 weaponScript)
    {
        OnWeaponSwitchEvent?.Invoke(weapon, weaponScript);
        OnWeaponSwitchUIUpdate?.Invoke(weaponScript.WeaponConfig.sprite, weaponScript.WeaponRuntimeData);
    }
    

    #endregion
}
