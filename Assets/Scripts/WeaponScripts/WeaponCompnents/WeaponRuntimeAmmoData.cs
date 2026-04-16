using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class WeaponRuntimeAmmoData
{
    public int RoundCapacity;
    public int CurrentAmmo;
    public int CurrentReserveAmmo;
    public int MaxReserveAmmo;
    public bool HasInfiniteReserveAmmo;

    public WeaponRuntimeAmmoData(WeaponAmmoConfigSO ammoConfig)
    {
        RoundCapacity = ammoConfig.RoundCapacity;
        CurrentAmmo = ammoConfig.RoundCapacity;
        MaxReserveAmmo = ammoConfig.MaxReserveAmmo;
        CurrentReserveAmmo = MaxReserveAmmo;
        HasInfiniteReserveAmmo = ammoConfig.HasInfiniteReserveAmmo;

        Debug.Log("Weapon Runtime Ammo Data CREATED with " + ammoConfig);
    }

    public bool HasAmmoToReload()
    {
        return HasInfiniteReserveAmmo || CurrentReserveAmmo != 0;
    }

    public int NeededAmmo()
    {
        // In case the weapon somehow has more CurrentAmmo than the RoundCapacity.
        return Mathf.Max(0, RoundCapacity - CurrentAmmo);
    }

    public bool CanReload()
    {
        if (CurrentAmmo != RoundCapacity && CurrentReserveAmmo != 0)
            return true;

        return false;
    }

}
