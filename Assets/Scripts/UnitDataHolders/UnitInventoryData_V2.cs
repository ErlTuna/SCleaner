using System;
using System.Collections.Generic;

[Serializable]
public class UnitInventoryData_V2
{
    public PlayerWeaponInventoryData WeaponInventory;
    public PlayerCurrencyInventoryData CurrencyInventory;
    public PlayerPassiveItemInventoryData PassiveItemInventory;

    public UnitInventoryData_V2() { }

    public static UnitInventoryData_V2 CreateNew(PlayerInventoryConfigSO config)
    {
        return new UnitInventoryData_V2
        {
            WeaponInventory = new PlayerWeaponInventoryData
            {
                MaxWeaponAmount = config.MaxWeaponAmount,
                Weapons = new List<WeaponInventoryEntry>()
            },

            CurrencyInventory = new PlayerCurrencyInventoryData
            {
                Current = 0,
                Max = config.MaxCurrency
            },
            
            PassiveItemInventory = new PlayerPassiveItemInventoryData()
        };
    }
}

/*

    PassiveItemInventory = new PlayerPassiveItemInventoryData(),

*/
