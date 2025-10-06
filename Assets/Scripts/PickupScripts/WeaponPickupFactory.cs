using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponPickupFactory
{
    // This method converts a dropped weapon into a weapon pickup
    public static GameObject CreatePickupFromWeapon(BaseWeapon weapon)
    {
        if (weapon == null || weapon.WeaponRuntimeData == null || weapon.WeaponRuntimeData.Config == null)
        {
            Debug.LogError("Cannot create pickup: invalid weapon or runtime data.");
            return null;
        }

        WeaponRuntimeData runtimeData = weapon.WeaponRuntimeData;
        WeaponConfigSO config = weapon.WeaponConfig;

        // + Vector3.down * 0.5f;
        Vector3 dropPosition = weapon.transform.position + Vector3.down * 0.5f;
        //Debug.Log("Dropping at ..." + dropPosition);
        GameObject pickupPrefab = config.PickupPrefab;

        GameObject pickupGO = GameObject.Instantiate(pickupPrefab, dropPosition, Quaternion.identity);

        if (pickupGO.TryGetComponent(out WeaponPickup pickupComponent) == false)
        {
            Debug.LogError("Pickup prefab is missing WeaponPickup script. Can't create.");
            GameObject.Destroy(pickupGO);
            return null;
        }

        pickupComponent.Initialize(runtimeData);
        return pickupGO;
    }

    // Might come in handy later if I ever decide to spawn fresh weapon pick ups
    public static WeaponPickup CreatePickupFromConfig(WeaponConfigSO config, Vector3 position)
    {
        if (config == null || config.PickupPrefab == null)
        {
            Debug.LogError("Missing config or pickup prefab.");
            return null;
        }

        GameObject pickupGO = GameObject.Instantiate(config.PickupPrefab, position, Quaternion.identity);
        WeaponPickup pickupComponent = pickupGO.GetComponent<WeaponPickup>();

        if (pickupComponent == null)
        {
            Debug.LogError("Pickup prefab missing WeaponPickup component.");
            GameObject.Destroy(pickupGO);
            return null;
        }

        pickupComponent.Initialize(config); // Initializes with just static data
        return pickupComponent;
    }
}


