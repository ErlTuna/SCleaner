public static class WeaponRuntimeFactory
{
    public static WeaponRuntime Create(WeaponConfigSO config)
    {
        WeaponAmmoData ammo = new(config.AmmoConfig);
        return new WeaponRuntime(config, ammo);
    }
}

