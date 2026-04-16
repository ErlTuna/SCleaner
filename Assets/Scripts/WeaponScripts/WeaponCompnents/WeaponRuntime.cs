public class WeaponRuntime
{
    readonly public WeaponConfigSO Config;
    public WeaponState State;
    WeaponAmmoData _weaponAmmoData;
    public WeaponAmmoData AmmoData => _weaponAmmoData;

    public bool isTriggerHeld;
    public bool CanBeDropped {get; private set;}

    public int Damage;
    public float TimeSinceLastFired;
    public float SpreadResetThreshold;


    public WeaponRuntime(WeaponConfigSO config, WeaponAmmoData ammoData)
    {
        if (config == null) return;

        Config = config;
        _weaponAmmoData = ammoData;


        State = WeaponState.IDLE;
        Damage = Config.Damage;
        TimeSinceLastFired = 0;
        SpreadResetThreshold = config.SpreadResetThreshold;
        
        CanBeDropped = config.CanBeDropped;

    }

}
