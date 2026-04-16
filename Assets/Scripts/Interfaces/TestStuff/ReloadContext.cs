public class ReloadContext
{
    public WeaponAmmoData AmmoRuntime {get; set;}
    public IAmmoConfig AmmoConfig {get; set;}

    public ReloadContext (WeaponAmmoData runtimeAmmoData, WeaponAmmoConfigSO ammoConfig)
    {
        AmmoRuntime = runtimeAmmoData;
        AmmoConfig = ammoConfig;
    }
}


