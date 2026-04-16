using System;

public static class WeaponEvents
{
    public static event Action<WeaponRuntime> OnWeaponFiredEvent;
    public static event Action<WeaponRuntime> OnWeaponReloadEvent;
    public static event Action<WeaponConfigSO> OnWeaponSwitchEvent;
    public static event Action OnWeaponOutOfAmmoEvent;
    public static void RaiseAmmoUsed(WeaponRuntime weaponData) => OnWeaponFiredEvent?.Invoke(weaponData);
    public static void RaiseWeaponReload(WeaponRuntime weaponData) => OnWeaponReloadEvent?.Invoke(weaponData);
    public static void RaiseWeaponSwitched(WeaponConfigSO weaponData) => OnWeaponSwitchEvent?.Invoke(weaponData);
    public static void RaiseWeaponOutOfAmmo() => OnWeaponOutOfAmmoEvent?.Invoke();
}
