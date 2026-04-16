public static class WeaponRuntimeFactory
{
    public static WeaponRuntimeData Create(WeaponConfigSO config)
    {
        WeaponRuntimeAmmoData ammo = new(config.AmmoConfig);
        return new WeaponRuntimeData(config, ammo);
    }
}

