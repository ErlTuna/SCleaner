using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerWeaponInventory
{
    // These channels are injected through the config SO
    readonly ItemPickedUpEventChannelSO _weaponAddedEventChannel;
    readonly ItemDroppedEventChannel _weaponDroppedEventChannel;
    public int WeaponsHeld => WeaponScripts.Count();
    public int MaxWeaponAmount {get; private set;}

    // TODO : Remove the stored GameObjects later if implementing save/load.
    private readonly List<PlayerWeapon> _weaponScripts = new();
    public IReadOnlyList<PlayerWeapon> WeaponScripts => _weaponScripts;


    public PlayerWeaponInventory(
                                int weaponsHeld, 
                                int maxWeaponAmount, 
                                ItemPickedUpEventChannelSO weaponInventoryUpdateEventChannel, 
                                ItemDroppedEventChannel weaponDropEventChannel
                              )
    {
        //WeaponsHeld = weaponsHeld;
        MaxWeaponAmount = maxWeaponAmount;
        _weaponAddedEventChannel = weaponInventoryUpdateEventChannel;
        _weaponDroppedEventChannel = weaponDropEventChannel;
    }

    // Inventory manager does the necessary checks through CanPickupWeapon
    public void AddWeaponToInventory(GameObject weapon, PlayerWeapon weaponScript)
    {
        //WeaponGOs.Add(weapon);
        _weaponScripts.Add(weaponScript);
        Debug.Log("Added weapon : " + weapon + " to inventory");
        //WeaponsHeld++;
        Debug.Log("Now holding : " + WeaponsHeld + " weapon(s)");

        if (_weaponAddedEventChannel != null)
        {
            //Pass the inventory item SO to inventory screen
            Debug.Log("Event raised for weapon added channel");
            _weaponAddedEventChannel.RaiseEvent(weaponScript.WeaponConfig.InventoryItemDefinition);
        }
            
    }

    // Inventory manager does the necessary checks
    public void RemoveWeaponFromInventory(GameObject weapon, PlayerWeapon weaponScript)
    {
        //WeaponGOs.Remove(weapon);
        _weaponScripts.Remove(weaponScript);
        //WeaponsHeld--;
        Debug.Log("Removed weapon : " + weapon + " from inventory. Now holding : " + WeaponsHeld + " weapon(s).");
        if (_weaponDroppedEventChannel != null)
        {
            // Pass item ID to inventory UI
            _weaponDroppedEventChannel.RaiseEvent(weaponScript.WeaponConfig.InventoryItemGUID);
        }

    }

    public bool CanPickupWeapon(PlayerWeapon weaponToPickUp)
    {   
        /*
        if (WeaponsHeld == MaxWeaponAmount)
        {
            Debug.Log("Can't pick up any more weapons.");
            return false;
        }
        */

        /*    
        if (HasWeapon(weaponToPickUp.PlayerWeaponConfig.InventoryItemGUID))
        {
            Debug.Log("This weapon is already owned. Can't pick it up.");
            return false;
        }
        */
        
        

        return true;
    }

    // Configs are unique per weapon. We can use that to check if a weapon exists.
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

    bool HasWeapon(string itemID)
    {
        return WeaponScripts.Any(w => w.WeaponConfig.InventoryItemGUID == itemID);
    }
    
    public bool IsEmpty()
    {
        return WeaponsHeld == 0;
    }

}
