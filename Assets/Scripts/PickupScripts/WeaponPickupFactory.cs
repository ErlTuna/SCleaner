using Unity.Mathematics;
using UnityEngine;

public static class WeaponPickupFactory
{
    public static void Create(WeaponRuntime weaponRuntimeData, Vector3 location)
    {
        if (weaponRuntimeData == null)
        {
            Debug.LogError("Weapon runtime data or config or pickup definition is missing. Cannot create pickup.");
            return;
        }

        PlayerWeaponConfigSO playerWeaponConfig = weaponRuntimeData.Config as PlayerWeaponConfigSO;

        if (playerWeaponConfig == null) return;
        
        GameObject pickupGO = GameObject.Instantiate(playerWeaponConfig.PickupPrefab);

        if (pickupGO.TryGetComponent(out WeaponPickup pickupScript) == false) return;

    
        Vector3 dropPosition = location + Vector3.down * 0.5f;

        pickupScript.InitializeWithExistingWeapon(weaponRuntimeData);
        pickupGO.transform.SetPositionAndRotation(dropPosition, Quaternion.identity);

    }

    public static void Create_v2(WeaponRuntime runtimeData, Vector3 location)
    {
        if (runtimeData?.Config is not PlayerWeaponConfigSO config) return;

        GameObject pickupGO = GameObject.Instantiate(config.PickupPrefab);
        if (pickupGO.TryGetComponent<IPayloadProvider>(out var provider))
        {
            if (provider is WeaponPickup weaponPickup)
                weaponPickup.InitializeWithExistingWeapon(runtimeData);
        }

        //pickupGO.transform.SetPositionAndRotation(location + Vector3.down * 0.5f, Quaternion.identity);
        pickupGO.transform.SetPositionAndRotation(location, Quaternion.identity);
    }
    
    
}




