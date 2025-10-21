using UnityEngine;

// Trakcs a weapon's ammunition states and handles reload requests
// Is the weapon fully loaded?
// Does the weapon have ammo in it currenty?
// Does the weapon have reserve ammo?
// Using ammunition
// Can the weapon reload?
// Actual handling of the reload requests are done by the ReloadStrategy object


public class AmmoManager : MonoBehaviour
{
    public BaseWeapon Owner;
    public ReloadStrategySO ReloadStrategy;

    public bool IsFullyLoaded()
    {
        return Owner.WeaponConfig.RoundCapacity - Owner.WeaponRuntimeData.CurrentAmmo == 0;
    }
    public bool HasAmmo()
    {
        return Owner.WeaponRuntimeData.CurrentAmmo > 0;
    }
    public bool HasReserveAmmo()
    {
        if (Owner.WeaponConfig.HasInfiniteReserveAmmo) return true;

        return Owner.WeaponRuntimeData.ReserveAmmo > 0;
    }
    public void UseAmmo()
    {
        if (Owner.WeaponRuntimeData.CurrentAmmo > 0)
            Owner.WeaponRuntimeData.CurrentAmmo--;

        //raise event to update UI
        WeaponEvents.RaiseWeaponFired(Owner.WeaponRuntimeData);
    }
    public bool CanReload()
    {
        return HasReserveAmmo() && !IsFullyLoaded();
    }
    public void HandleReloadStart()
    {
        ReloadStrategy.ReloadStart(Owner);
    }
    public void UseReloadStrategy()
    {
        ReloadStrategy.PerformReload(Owner);
        //Update UI
        WeaponEvents.RaiseWeaponReload(Owner.WeaponRuntimeData);
    }
    public void ShouldContinueReloading()
    {
        ReloadStrategy.HandleReloadContinuation(Owner);
    }
}

