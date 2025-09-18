using System;
using UnityEngine;

public class AmmoTracker
{
    BaseWeapon_v2 _owner;
    

    public int CurrentAmmo
    {
        get => _owner.WeaponRuntimeData.CurrentAmmo;
        set => _owner.WeaponRuntimeData.CurrentAmmo = value;
    }

    public int ReserveAmmo
    {
        get => _owner.WeaponRuntimeData.ReserveAmmo;
        set => _owner.WeaponRuntimeData.ReserveAmmo = value;
    }

    public int RoundCapacity => _owner.WeaponConfig.RoundCapacity;

    public AmmoTracker() { }

    public AmmoTracker(BaseWeapon_v2 owner)
    {
        _owner = owner;
    }

    public bool HasAmmo() => CurrentAmmo > 0;

    public void UseAmmo()
    {
        if (CurrentAmmo > 0)
            CurrentAmmo--;

        WeaponEvents.RaiseWeaponFired(_owner.WeaponRuntimeData);
    }

}
