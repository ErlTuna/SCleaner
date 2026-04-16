using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeaponInventoryRuntime
{
    readonly PlayerWeaponInventoryData _data;
    readonly WeaponDatabaseSO _database;

    readonly ItemPickedUpEventChannelSO _weaponAddedEventChannel;
    readonly ItemDroppedEventChannel _weaponDroppedEventChannel;

    readonly List<WeaponInstance> _weaponInstances = new();
    public IReadOnlyList<WeaponInstance> Weapons => _weaponInstances;
    readonly Dictionary<string, WeaponInstance> _weaponLookup = new();

    public int WeaponsHeld => _weaponInstances.Count;
    public int MaxWeaponAmount => _data.MaxWeaponAmount;

    public PlayerWeaponInventoryRuntime(PlayerWeaponInventoryData data, WeaponInventoryDependencies dependencies)
    {
        _data = data;
        _database = dependencies.Database;
        _weaponAddedEventChannel = dependencies.AddedChannel;
        _weaponDroppedEventChannel = dependencies.DroppedChannel;
    }


    public WeaponInstance AddWeaponToInventory(GameObject weaponGO, PlayerWeapon weaponScript)
    {
        string id = weaponScript.WeaponConfig.InventoryItemGUID;

        WeaponInstance weaponInstance = CreateInstance(id, weaponGO, weaponScript);
        WeaponInventoryEntry entry = new()
        {
            WeaponID = id,
            CurrentAmmo = weaponScript.WeaponRuntimeData.AmmoData.CurrentAmmo,
            CurrentReserveAmmo = weaponScript.WeaponRuntimeData.AmmoData.CurrentReserveAmmo
        };
        
        _data.Weapons.Add(entry);

        Debug.Log($"Added weapon {id}");


        _weaponAddedEventChannel?.RaiseEvent(
            weaponScript.WeaponConfig.InventoryItemDefinition
        );

        return weaponInstance;
    }


    public void RemoveWeaponFromInventory(string id)
    {
        //WeaponInstance instance = _weaponLookup[id];
        RemoveWeaponById(id);

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
        return _weaponLookup.ContainsKey(weaponID);
    }

    public bool IsEmpty()
    {
        Debug.Log("How many weapons am I holding?.." + WeaponsHeld);
        return WeaponsHeld == 0;
    }


    // To be used when loading existing data.
    public void RebuildFromData()
    {
        _weaponInstances.Clear();
        _weaponLookup.Clear();

        foreach (WeaponInventoryEntry entry in _data.Weapons)
        {
            PlayerWeaponConfigSO config = _database.Get(entry.WeaponID);
            if (config == null)
            {
                Debug.LogWarning($"Weapon ID not found: {entry.WeaponID}");
                continue;
            }
    
            (PlayerWeapon weaponScript, GameObject weaponGO) = WeaponFactory.CreateUsingConfig(config);
            CreateInstance(entry.WeaponID, weaponGO, weaponScript);
        }
    }

    WeaponInstance CreateInstance(string id, GameObject weaponGO, PlayerWeapon weaponScript)
    {
        WeaponInstance instance = new(id, weaponGO, weaponScript);

        _weaponInstances.Add(instance);
        _weaponLookup[id] = instance;

        return instance;
    }

    public void RemoveWeaponById(string id)
    {
        _data.Weapons.RemoveAll(w => w.WeaponID == id);
        _weaponInstances.RemoveAll(i => i.WeaponID == id);
        _weaponLookup.Remove(id);
    }
}
