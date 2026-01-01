using System;
using System.Linq;

[Serializable]
public class UnitInventoryData
{
    public UnitWeaponInventory WeaponInventory {get; private set;}
    public UnitEquipmentInventory EquipmentInventory {get; private set;}
    public UnitCurrencyInventory CurrencyInventory {get; private set;}
    public UnitInventoryData(UnitInventoryConfigSO config)
    {
        // These event channels can be bundled in a struct later
        WeaponInventory = new(config.WeaponPrefabs.Count(), config.MaxWeaponAmount, config.WeaponAddedEventChannel, config.WeaponDropEventChannel);
        
        EquipmentInventory = new UnitEquipmentInventory(config.EquipmentPickedUpChannel, config.EquipmentDroppedChannel);
        CurrencyInventory = new UnitCurrencyInventory
        {
            OwnedCurrency = 0,
            MaxCurrency = config.MaxCurrency
        };
    }


}
