public class ReloadContext
{
    public WeaponRuntimeAmmoData AmmoRuntime {get; set;}
    public IAmmoConfig AmmoConfig {get; set;}

    public ReloadContext (WeaponRuntimeAmmoData runtimeAmmoData, WeaponAmmoConfigSO ammoConfig)
    {
        AmmoRuntime = runtimeAmmoData;
        AmmoConfig = ammoConfig;
    }
}


