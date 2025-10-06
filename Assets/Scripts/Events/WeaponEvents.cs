using System;

public static class WeaponEvents
{
    public static event Action<WeaponRuntimeData> OnWeaponFiredEvent;
    public static event Action<WeaponRuntimeData> OnWeaponReloadEvent;
    public static event Action<WeaponConfigSO> OnWeaponSwitchEvent;
    public static event Action OnWeaponOutOfAmmoEvent;
    public static void RaiseWeaponFired(WeaponRuntimeData weaponData) => OnWeaponFiredEvent?.Invoke(weaponData);
    public static void RaiseWeaponReload(WeaponRuntimeData weaponData) => OnWeaponReloadEvent?.Invoke(weaponData);
    public static void RaiseWeaponSwitched(WeaponConfigSO weaponData) => OnWeaponSwitchEvent?.Invoke(weaponData);
    public static void RaiseWeaponOutOfAmmo() => OnWeaponOutOfAmmoEvent?.Invoke();
}
