using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInventoryEvents
{
    #region Events
    public static Action<GameObject, PlayerWeapon> OnWeaponsReadyEvent;
    public static Action<GameObject, PlayerWeapon> OnWeaponSwitchEvent;
    public static Action<Sprite, WeaponRuntimeData> OnWeaponSwitchUIUpdate;
    public static Action<GameObject, BaseEquipment> OnEquipmentReady;
    public static Action<Sprite, EquipmentData> OnEquipmentSwitchUIUpdate;
    public static Action<GameObject, BaseEquipment> OnEquipmentSwitchEvent;
    //public static Action<GameObject, BaseEquipment> OnEquipmentSwitchUIUpdate;

    #endregion

    #region Invokers

    public static void RaiseWeaponsReadyEvent(GameObject weapon, PlayerWeapon weaponScript)
    {
        OnWeaponsReadyEvent?.Invoke(weapon, weaponScript);
    }

    public static void RaiseEquipmentReadyEvent(GameObject equipment, BaseEquipment eqipmentScript)
    {
        OnEquipmentReady?.Invoke(equipment, eqipmentScript);
    }


    public static void RaiseWeaponSwitchEvent(GameObject weapon, PlayerWeapon weaponScript)
    {
        OnWeaponSwitchEvent?.Invoke(weapon, weaponScript);
        if (weaponScript)
            OnWeaponSwitchUIUpdate?.Invoke(weaponScript.WeaponConfig.UI_Icon, weaponScript.WeaponRuntimeData);

        else
            OnWeaponSwitchUIUpdate?.Invoke(null, null);
        
    }

    public static void RaiseEquipmentSwitchEvent(GameObject equipment, BaseEquipment equipmentScript)
    {
        
    }

    public static void RaiseEquipmentSwitchUIUpdateEvent(Sprite equipmentSprite, EquipmentData equipmentData)
    {
        OnEquipmentSwitchUIUpdate?.Invoke(equipmentSprite, equipmentData);
    }
    

    #endregion
}
