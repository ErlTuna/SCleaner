using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInventoryEvents
{
    #region Events
    public static Action<GameObject, PlayerWeapon> OnInventoryReadyEvent;
    public static Action<GameObject, PlayerWeapon> OnWeaponSwitchEvent;
    public static Action<Sprite, WeaponRuntimeData> OnWeaponSwitchUIUpdate;
    public static Action<GameObject, BaseEquipment> OnEquipmentReady;
    public static Action<GameObject, BaseEquipment> OnEquipmentSwitchEvent;
    public static Action<GameObject, BaseEquipment> OnEquipmentSwitchUIUpdate;

    #endregion

    #region Invokers

    public static void RaiseInventoryReadyEvent(GameObject weapon, PlayerWeapon weaponScript)
    {
        OnInventoryReadyEvent?.Invoke(weapon, weaponScript);
    }

    public static void RaiseEquipmentReadyEvent(GameObject equipment, BaseEquipment eqipmentScript)
    {
        OnEquipmentReady?.Invoke(equipment, eqipmentScript);
    }


    public static void RaiseWeaponSwitchEvent(GameObject weapon, PlayerWeapon weaponScript)
    {
        OnWeaponSwitchEvent?.Invoke(weapon, weaponScript);
        if (weaponScript)
            OnWeaponSwitchUIUpdate?.Invoke(weaponScript.WeaponConfig.Sprite, weaponScript.WeaponRuntimeData);

        else
            OnWeaponSwitchUIUpdate?.Invoke(null, null);
        
    }

    public static void RaiseEquipmentSwitchEvent(GameObject equipment, BaseEquipment equipmentScript)
    {
        
    }
    

    #endregion
}
