using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInventoryEvents
{
    #region Events
    public static Action<GameObject, BaseWeapon> OnInventoryReadyEvent;
    public static Action<GameObject, BaseWeapon> OnWeaponSwitchEvent;
    public static Action<Sprite, WeaponRuntimeData> OnWeaponSwitchUIUpdate;
    public static Action<GameObject, BaseEquipment> OnEquipmentReady;
    public static Action<GameObject, BaseEquipment> OnEquipmentSwitchEvent;
    public static Action<GameObject, BaseEquipment> OnEquipmentSwitchUIUpdate;

    #endregion

    #region Invokers

    public static void RaiseInventoryReadyEvent(GameObject weapon, BaseWeapon weaponScript)
    {
        OnInventoryReadyEvent?.Invoke(weapon, weaponScript);
        OnWeaponSwitchUIUpdate?.Invoke(weaponScript.WeaponConfig.sprite, weaponScript.WeaponRuntimeData);
    }

    public static void RaiseEquipmentReadyEvent(GameObject equipment, BaseEquipment eqipmentScript)
    {
        OnEquipmentReady?.Invoke(equipment, eqipmentScript);
    }


    public static void RaiseWeaponSwitchEvent(GameObject weapon, BaseWeapon weaponScript)
    {
        OnWeaponSwitchEvent?.Invoke(weapon, weaponScript);
        OnWeaponSwitchUIUpdate?.Invoke(weaponScript.WeaponConfig.sprite, weaponScript.WeaponRuntimeData);
    }

    public static void RaiseEquipmentSwitchEvent(GameObject equipment, BaseEquipment equipmentScript)
    {
        
    }
    

    #endregion
}
