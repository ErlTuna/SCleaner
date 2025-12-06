using System;
using System.Linq;

[Serializable]
public class UnitInventoryData
{
    public UnitWeaponInventory WeaponInventory;
    public UnitEquipmentInventory EquipmentInventory;
    public UnitCurrencyInventory CurrencyInventory;
    public UnitInventoryData(UnitInventoryConfigSO config)
    {
        WeaponInventory = new UnitWeaponInventory
        {
            MaxWeaponAmount = config.MaxWeaponAmount,
            WeaponsHeld = config.WeaponPrefabs.Count()
        };

        EquipmentInventory = new UnitEquipmentInventory();
        CurrencyInventory = new UnitCurrencyInventory
        {
            OwnedCurrency = config.OwnedCurrency,
            MaxCurrency = config.MaxCurrency
        };
    }


}
