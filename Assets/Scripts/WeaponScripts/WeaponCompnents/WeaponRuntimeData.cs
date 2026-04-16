using System.Collections.Generic;

//[Serializable]
public class WeaponRuntimeData
{
    readonly public List<WeaponSpreadModifier> SpreadModifiers;
    readonly public List<WeaponClipSizeModifier> ClipModifiers;
    readonly public List<WeaponDamageModifier> DamageModifiers;
    private readonly HashSet<IWeaponModifier> _activeModifiers = new();
    readonly public WeaponConfigSO Config;
    public WeaponState State;
    WeaponRuntimeAmmoData _weaponAmmoRuntimeData;
    public WeaponRuntimeAmmoData AmmoData => _weaponAmmoRuntimeData;

    public string WeaponID {get; private set;}
    public bool isTriggerHeld;
    public bool CanBeDropped {get; private set;}

    public int Damage;
    public float TimeSinceLastFired;
    public float SpreadResetThreshold;


    public WeaponRuntimeData(WeaponConfigSO config, WeaponRuntimeAmmoData ammoData)
    {
        if (config == null) return;

        Config = config;
        _weaponAmmoRuntimeData = ammoData;


        State = WeaponState.IDLE;
        Damage = Config.Damage;
        TimeSinceLastFired = 0;
        SpreadResetThreshold = config.SpreadResetThreshold;
        
        CanBeDropped = config.CanBeDropped;

        SpreadModifiers = new();
        ClipModifiers = new();
        DamageModifiers = new();


        // Sort this out later
        PlayerWeaponConfigSO playerWeaponConfigSO = config as PlayerWeaponConfigSO;

        WeaponID = playerWeaponConfigSO.InventoryItemGUID;

    }

    /*
    public void SetAcquisitionSource(ItemAcquisitionSource acquisitionSource)
    {
        //AcquisitionSource = acquisitionSource;
    }
    */

    public void ApplyClipSizeModifier(WeaponClipSizeModifier modifier)
    {
        if (_activeModifiers.Contains(modifier)) return;

        _activeModifiers.Add(modifier);
        //RoundCapacity += modifier.Amount;
    }

    public void RemoveClipSizeModifier(WeaponClipSizeModifier modifier)
    {
        if (_activeModifiers.Remove(modifier) != true) return;

        //RoundCapacity -= modifier.Amount;
    }
}
