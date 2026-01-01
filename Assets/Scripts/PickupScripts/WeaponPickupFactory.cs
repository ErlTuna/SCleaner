using UnityEngine;

public static class WeaponPickupFactory
{
    public static WeaponPickup Create(WeaponRuntimeData runtimeData, Vector3 location)
    {
        if (runtimeData == null || runtimeData.Config == null || runtimeData.Config.PickupDefinition == null)
        {
            Debug.LogError("Weapon runtime data or config or pickup definition is missing. Cannot create pickup.");
            return null;
        }

        WeaponPickupDefinitionSO definition = runtimeData.Config.PickupDefinition;
        GameObject prefab = definition.PickupPrefab;

        Vector3 dropPosition = location + Vector3.down * 0.5f;

        GameObject pickupGO = Object.Instantiate(prefab, dropPosition, Quaternion.identity);

        if (!pickupGO.TryGetComponent(out WeaponPickup pickup))
        {
            Debug.LogError("Pickup prefab is missing WeaponPickup.");
            Object.Destroy(pickupGO);
            return null;
        }

        pickup.Initialize(definition, runtimeData);
        return pickup;
    }
    
}


