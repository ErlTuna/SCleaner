using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerInventoryData : IUnitInventoryData
{
    public int OwnedCurrency { get; set; }
    public int MaxCurrency { get; set; }
    public int WeaponsHeld { get; set; }
    public List<BaseWeapon_v2> WeaponScripts { get; set; }
    public List<GameObject> WeaponGOs { get; set; }
    public List<GameObject> weaponPrefabs { get; set; }
    List<OLD_BaseWeapon> IUnitInventoryData.WeaponScripts { get; set; }

    public void AutoConfigureWithWrapper(UnitConfigsWrapperSO config)
    {
        ConfigureWith(config.InventoryConfig);
    }

    public void ConfigureWith(UnitInventoryConfigSO config)
    {
        OwnedCurrency = config.currency;
        MaxCurrency = config.maxCurrency;
        WeaponGOs = new();
        WeaponScripts = new();
        weaponPrefabs = config.weaponPrefabs;
        WeaponsHeld = weaponPrefabs.Count();
        //WeaponConfigs = new List<WeaponConfig>(config.weaponConfigs);
    }


}
