using System;
using System.Collections.Generic;

[Serializable]
public class UnitInventoryData_V2
{
    public PlayerWeaponInventoryData WeaponInventory;
    //public UnitCurrencyInventoryData CurrencyInventory;
    //public PlayerPassiveItemInventoryData PassiveItemInventory;

    public UnitInventoryData_V2() { }

    public static UnitInventoryData_V2 CreateNew(PlayerInventoryConfigSO config)
    {
        return new UnitInventoryData_V2
        {
            WeaponInventory = new PlayerWeaponInventoryData
            {
                MaxWeaponAmount = config.MaxWeaponAmount,
                WeaponIDs = new List<string>()
            }
        };
    }
}

/*
,
        
        CurrencyInventory = new UnitCurrencyInventoryData
        {
            Current = 0,
            Max = config.MaxCurrency
        },

        PassiveItemInventory = new PlayerPassiveItemInventoryData(),

*/
