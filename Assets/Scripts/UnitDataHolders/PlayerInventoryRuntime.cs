public class PlayerInventoryRuntime
{
    public PlayerWeaponInventoryRuntime WeaponInventoryRuntime { get; private set; }
    //public PlayerEquipmentInventory EquipmentInventory { get; private set; }
    public PlayerCurrencyInventoryRuntime CurrencyInventoryRuntime { get; private set; }
    public PlayerPassiveItemInventoryRuntime PassiveItemInventoryRuntime { get; private set; }

    public PlayerInventoryRuntime(UnitInventoryData_V2 data, InventoryRuntimeDependencies dependencies)
    {
        WeaponInventoryRuntime = new(data.WeaponInventory, dependencies.WeaponInventoryDependencies);
        CurrencyInventoryRuntime = new(data.CurrencyInventory, dependencies.CurrencyInventoryDependencies);
        PassiveItemInventoryRuntime = new(data.PassiveItemInventory, dependencies.PassiveItemInventoryDependencies);
    }
}

/*
        PassiveItemInventory = new PlayerPassiveItemInventory(
            data.PassiveItemInventory,
            config.ItemPickedUpEventChannel,
            config.ItemDroppedEventChannel
        );
        */