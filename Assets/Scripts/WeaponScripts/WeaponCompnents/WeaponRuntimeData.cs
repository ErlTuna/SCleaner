using System;
using UnityEngine;

[Serializable]
public class WeaponRuntimeData : IInventoryAddable
{
    readonly public WeaponConfigSO Config;
    public WeaponState State;
    public int CurrentAmmo;
    public int ReserveAmmo;
    public int Damage;
    public float TimeSinceLastFired;
    public float SpreadResetThreshold;


    public WeaponRuntimeData(WeaponConfigSO config)
    {
        if (config == null) return;

        Config = config;

        CurrentAmmo = Config.RoundCapacity;
        ReserveAmmo = Config.StartingReserveAmmo;
        State = WeaponState.IDLE;
        Damage = Config.Damage;
        TimeSinceLastFired = 0;
        SpreadResetThreshold = config.SpreadResetThreshold;
    }

    public bool CanBeAdded(PlayerInventoryManager inventory)
    {
        return inventory.CanPickupWeapon(Config);
    }

    public void AddToInventory(PlayerInventoryManager inventory)
    {
        inventory.TryAddWeapon(this);
    }
}
