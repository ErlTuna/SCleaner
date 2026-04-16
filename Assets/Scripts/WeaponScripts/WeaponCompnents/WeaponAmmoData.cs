using UnityEngine;

[System.Serializable]
public class WeaponAmmoData
{
    public int RoundCapacity;
    public int CurrentAmmo;
    public int CurrentReserveAmmo;
    public int MaxReserveAmmo;
    public bool HasInfiniteReserveAmmo;

    public WeaponAmmoData(WeaponAmmoConfigSO ammoConfig)
    {
        RoundCapacity = ammoConfig.RoundCapacity;
        CurrentAmmo = ammoConfig.RoundCapacity;
        MaxReserveAmmo = ammoConfig.MaxReserveAmmo;
        CurrentReserveAmmo = MaxReserveAmmo;
        HasInfiniteReserveAmmo = ammoConfig.HasInfiniteReserveAmmo;

        Debug.Log("Weapon Runtime Ammo Data CREATED with " + ammoConfig);
    }
}
