using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// NEEDS SPLITTING UP!

[Serializable]
public class UnitInventoryData : IUnitInventoryData
{
    public int OwnedCurrency { get; set; }
    public int MaxCurrency { get; set; }
    public int WeaponsHeld { get; set; }
    public int EquipmentsHeld { get; set; }
    public List<BaseWeapon> WeaponScripts { get; set; }
    public List<GameObject> WeaponGOs { get; set; }
    public List<GameObject> WeaponPrefabs { get; set; }
    public List<GameObject> EquipmentPrefabs { get; set; }
    public List<GameObject> EquipmentGOs { get; set; }
    public List<BaseEquipment> EquipmentScripts { get; set; }
    public UnitWeaponInventory WeaponInventory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public UnitEquipmentInventory EquipmentInventory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public UnitCurrencyInventory CurrencyInventory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void AutoConfigureWithWrapper(UnitConfigsWrapperSO config)
    {
        ConfigureWith(config.InventoryConfig);
    }

    public void ConfigureWith(UnitInventoryConfigSO config)
    {
        OwnedCurrency = config.currency;
        MaxCurrency = config.maxCurrency;

        WeaponPrefabs = config.weaponPrefabs;
        WeaponsHeld = WeaponPrefabs.Count();
        WeaponGOs = new();
        WeaponScripts = new();

        EquipmentPrefabs = config.equipmentPrefabs;
        EquipmentsHeld = EquipmentPrefabs.Count();
        EquipmentGOs = new();
        EquipmentScripts = new();
    }


}
