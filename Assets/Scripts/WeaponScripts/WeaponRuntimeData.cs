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
    public int PelletCount;


    public WeaponRuntimeData(WeaponConfigSO config)
    {
        //Debug.Log("uh ok!!");
        Config = config;
        CurrentAmmo = config.RoundCapacity;
        ReserveAmmo = config.StartingReserveAmmo;
        State = config.State;
        PelletCount = config.BulletPerShot;
    }
}
