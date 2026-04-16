using UnityEngine;


public static class WeaponFactory
{
    // Instantiates the weapon game object using the runtime data, returns the game object and associated script.
    public static (PlayerWeapon weaponScript, GameObject weaponGO) CreateUsingRuntimeData(WeaponRuntime runtimeData)
    {
        if (runtimeData == null || runtimeData.Config == null)
            return (null, null); 

        GameObject weaponGO = GameObject.Instantiate(runtimeData.Config.Prefab);
        if (weaponGO == null) 
            return (null, null);

        if (weaponGO.TryGetComponent<PlayerWeapon>(out var weaponScript) == false) 
        {
            GameObject.Destroy(weaponGO); // If for some reason the Weapon GO doesn't have a weaponScript
            return (null, null);
        }

        weaponScript.InitializeWithRuntimeData(runtimeData);
        return (weaponScript, weaponGO);
    }

    // Instantiates the weapon game object using the config and returns the game object and its script.
    // Every weapon config should have a valid weapon prefab.
    public static (PlayerWeapon weaponScript, GameObject weaponGO) CreateUsingConfig(WeaponConfigSO weaponConfig)
    {
        if (weaponConfig == null || weaponConfig.AmmoConfig == null || weaponConfig.Prefab == null)
            return (null, null);

        GameObject weaponGO = GameObject.Instantiate(weaponConfig.Prefab);
        if (weaponGO == null) 
            return (null, null);

        if (weaponGO.TryGetComponent<PlayerWeapon>(out var weaponScript) == false) 
        {
            GameObject.Destroy(weaponGO); // If for some reason the Weapon GO doesn't have a weaponScript
            return (null, null);
        }

        WeaponRuntime runtimeData = WeaponRuntimeFactory.Create(weaponConfig);
        weaponScript.InitializeWithRuntimeData(runtimeData);
        
        return (weaponScript, weaponGO);
    }
}

