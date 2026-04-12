using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeaponInventoryRuntime
{
    private readonly PlayerWeaponInventoryData _data;
    private readonly WeaponDatabaseSO _database;

    private readonly ItemPickedUpEventChannelSO _weaponAddedEventChannel;
    private readonly ItemDroppedEventChannel _weaponDroppedEventChannel;

    readonly List<WeaponInstance> _weaponInstances = new();
    public IReadOnlyList<WeaponInstance> Weapons => _weaponInstances;

    public int WeaponsHeld => _weaponInstances.Count;
    public int MaxWeaponAmount => _data.MaxWeaponAmount;

    public PlayerWeaponInventoryRuntime(PlayerWeaponInventoryData data, WeaponInventoryDependencies dependencies)
    {
        _data = data;
        _database = dependencies.Database;
        _weaponAddedEventChannel = dependencies.AddedChannel;
        _weaponDroppedEventChannel = dependencies.DroppedChannel;
    }


    public void AddWeaponToInventory(GameObject weaponGO, PlayerWeapon weaponScript)
    {
        string id = weaponScript.WeaponConfig.InventoryItemGUID;

        // 1. Update DATA (persistent state)
        _data.WeaponIDs.Add(id);

        // 2. Update RUNTIME (scene state)
        WeaponInstance weaponInstance = new(id, weaponGO, weaponScript);

        _weaponInstances.Add(weaponInstance);
        //_activeWeapons.Add(weaponScript);
        //_weaponGOs.Add(weaponGO);

        Debug.Log($"Added weapon {id}");

        // 3. Events (UI / feedback)
        _weaponAddedEventChannel?.RaiseEvent(
            weaponScript.WeaponConfig.InventoryItemDefinition
        );
    }

    /*
    public WeaponInstance AddWeapon(string id)
    {
        // 1. Resolve definition
        WeaponConfigSO config = _database.Get(id);
        if (config == null)
            return null;
    
        // 2. Create runtime data (identity copied here)
        WeaponRuntimeData runtimeData = new(
            id,
            config
        );
    
        // 3. Ask factory to create scene objects
        (PlayerWeapon weapon, GameObject go) =
            WeaponFactory.CreateUsingRuntimeData(runtimeData);
    
        // 4. Build instance (runtime truth)
        WeaponInstance instance = new WeaponInstance(
            id,
            go,
            weapon
        );
    
        // 5. Update state
        _data.WeaponIDs.Add(id);
        _weaponInstances.Add(instance);
    
        // 6. Notify systems
        _weaponAddedEventChannel?.RaiseEvent(config.InventoryItemDefinition);
    
        return instance;
    }
    */

    public void RemoveWeaponFromInventory(GameObject weaponGO, PlayerWeapon weaponScript)
    {
        string id = weaponScript.WeaponConfig.InventoryItemGUID;
        WeaponInstance instance = _weaponInstances.FirstOrDefault(w => w.WeaponID == id);

        _data.WeaponIDs.Remove(id);
        _weaponInstances.Remove(instance);
        //_activeWeapons.Remove(weaponScript);
        //_weaponGOs.Remove(weaponGO);

        Debug.Log($"Removed weapon {id}");

        if (_weaponDroppedEventChannel)
            _weaponDroppedEventChannel.RaiseEvent(id);
    }

    public bool CanPickupWeapon(PlayerWeaponConfigSO weaponToPickUp)
    {   
        /*
        if (WeaponsHeld == MaxWeaponAmount)
        {
            Debug.Log("Can't pick up any more weapons.");
            return false;
        }
        */

        // Each WeaponConfig has an ID.
        if (weaponToPickUp == null)
        {
            Debug.Log("Weapon to pickup is null...");
            return false;
        }
        if (HasWeapon(weaponToPickUp.InventoryItemGUID))
        {
            Debug.Log("This weapon is already owned. Can't pick it up.");
            return false;
        }
        
        return true;
    }

    bool HasWeapon(string weaponID)
    {
        return _data.WeaponIDs.Contains(weaponID);
    }

    public bool IsEmpty()
    {
        return WeaponsHeld == 0;
    }

    public void RebuildFromData(Transform weaponParent)
    {
        //_activeWeapons.Clear();
        _weaponInstances.Clear();

        foreach (string id in _data.WeaponIDs)
        {
            PlayerWeaponConfigSO config = _database.Get(id);
            if (config == null)
            {
                Debug.LogWarning($"Weapon ID not found: {id}");
                continue;
            }
    
            (PlayerWeapon weaponScript, GameObject weaponGO) = WeaponFactory.CreateUsingConfig(config);

            //GameObject weaponGO = Object.Instantiate(config.Prefab, weaponParent);
            //PlayerWeapon weaponScript = weaponGO.GetComponent<PlayerWeapon>();

            WeaponInstance weaponInstance = new(id, weaponGO, weaponScript);
            _weaponInstances.Add(weaponInstance);
        }
    }

    public void AddIDTest(string id, Transform weaponParent)
    {
        PlayerWeaponConfigSO config = _database.Get(id);
        if (config == null)
            {
                Debug.LogWarning($"Weapon ID not found: {id}");
                return;
            }

        GameObject weaponGO = Object.Instantiate(config.Prefab, weaponParent);
        PlayerWeapon weaponScript = weaponGO.GetComponent<PlayerWeapon>();

        WeaponInstance weaponInstance = new(id, weaponGO, weaponScript);
        _weaponInstances.Add(weaponInstance);

    }
}
