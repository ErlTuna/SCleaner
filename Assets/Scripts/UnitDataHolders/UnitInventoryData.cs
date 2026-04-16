using System;
using System.Linq;

// Name this UnitInventoryRuntime later
[Serializable]
public class UnitInventoryData
{   
    // OLD
    public PlayerWeaponInventory WeaponInventory {get; private set;}

    // REFACTOR
    public PlayerEquipmentInventory EquipmentInventory {get; private set;}
    public PlayerCurrencyInventoryRuntime CurrencyInventory {get; private set;}
    public PlayerPassiveItemInventory PassiveItemInventory {get; private set;}

    public UnitInventoryData(PlayerInventoryConfigSO config)
    {   
        // config.WeaponPrefabs.Count()
        WeaponInventory = new(1, config.MaxWeaponAmount, config.WeaponAddedEventChannel, config.WeaponDropEventChannel);

        //WeaponInventory = new(1, config.MaxWeaponAmount, config.WeaponAddedEventChannel, config.WeaponDropEventChannel);
        
        //EquipmentInventory = new UnitEquipmentInventory(config.ItemPickedUpEventChannel, config.ItemDroppedEventChannel);
        //CurrencyInventory = new UnitCurrencyInventory(0, config.MaxCurrency, config.CurrencyPickedUpEventChannel);

        PassiveItemInventory = new(config.ItemPickedUpEventChannel, config.ItemDroppedEventChannel);
    }


}

// Manager will create the runtime systems, composition root will inject the data.
// Make sure to save UnitInventoryData, NOT the runtime data.
// Need methods to rebuild from data. Rebuild should happen at the correct level
