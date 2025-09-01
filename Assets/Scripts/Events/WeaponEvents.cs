using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponEvents
{
    public static event Action<WeaponData> OnWeaponFiredEvent;
    public static event Action<WeaponData> OnWeaponReloadEvent;
    public static event Action<WeaponData> OnWeaponSwitchEvent;
    public static void RaiseWeaponFired(WeaponData weaponData) => OnWeaponFiredEvent?.Invoke(weaponData);
    public static void RaiseWeaponReload(WeaponData weaponData) => OnWeaponReloadEvent?.Invoke(weaponData);
    public static void RaiseWeaponSwitched(WeaponData weaponData) => OnWeaponSwitchEvent?.Invoke(weaponData);
}
