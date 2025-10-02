using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UnitInventoryData : IUnitInventoryData
{
    public UnitInventoryConfigSO InventoryConfig;
    public UnitWeaponInventory WeaponInventory { get; set; }    
    public UnitEquipmentInventory EquipmentInventory { get; set; }
    public UnitCurrencyInventory CurrencyInventory { get; set; }

    public void AutoConfigureWithWrapper(UnitConfigsWrapperSO config)
    {
        ConfigureWith(config.InventoryConfig);
    }

    public void ConfigureWith(UnitInventoryConfigSO config)
    {
        InventoryConfig = config;
        Debug.Log("InventoryConfig state : " + InventoryConfig);
        WeaponInventory = new UnitWeaponInventory();
        EquipmentInventory = new UnitEquipmentInventory();
        CurrencyInventory = new UnitCurrencyInventory();    
    

        CurrencyInventory.OwnedCurrency = InventoryConfig.OwnedCurrency;
        CurrencyInventory.MaxCurrency = InventoryConfig.MaxCurrency;

        WeaponInventory.MaxWeaponAmount = InventoryConfig.MaxWeaponAmount;
        WeaponInventory.WeaponsHeld = InventoryConfig.WeaponPrefabs.Count();
        EquipmentInventory.EquipmentsHeld = InventoryConfig.EquipmentPrefabs.Count();

    }


}
