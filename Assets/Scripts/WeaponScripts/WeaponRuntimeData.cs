using System;
using UnityEngine;

[Serializable]
public class WeaponRuntimeData
{
    public WeaponState State;
    public WeaponConfigSO Config;
    public int CurrentAmmo;
    public int ReserveAmmo;
    public int Damage;


    public WeaponRuntimeData(WeaponConfigSO config)
    {
        Config = config;
        CurrentAmmo = config.RoundCapacity;
        ReserveAmmo = config.StartingReserveAmmo;
        State = config.State;
        Damage = config.Damage;
    }
}
