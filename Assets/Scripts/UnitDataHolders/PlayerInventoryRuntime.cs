using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInventoryRuntime
{
    public PlayerWeaponInventoryRuntime WeaponInventory { get; private set; }
    public PlayerEquipmentInventory EquipmentInventory { get; private set; }
    public UnitCurrencyInventory CurrencyInventory { get; private set; }
    public PlayerPassiveItemInventory PassiveItemInventory { get; private set; }

    public UnitInventoryRuntime(UnitInventoryData_V2 data, WeaponInventoryDependencies dependencies)
    {
        WeaponInventory = new PlayerWeaponInventoryRuntime(data.WeaponInventory, dependencies);

        
    }
}

/*
        CurrencyInventory = new UnitCurrencyInventory(
            data.CurrencyInventory,
            config.MaxCurrency,
            config.CurrencyPickedUpEventChannel
        );

        PassiveItemInventory = new PlayerPassiveItemInventory(
            data.PassiveItemInventory,
            config.ItemPickedUpEventChannel,
            config.ItemDroppedEventChannel
        );
        */