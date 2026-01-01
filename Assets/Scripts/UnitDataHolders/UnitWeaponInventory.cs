using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UnitWeaponInventory
{
    // These channels are injected through the config SO
    readonly ItemPickedUpEventChannel _weaponAddedEventChannel;
    readonly ItemDroppedEventChannel _weaponDroppedEventChannel;
    public int WeaponsHeld => WeaponScripts.Count();
    public int MaxWeaponAmount {get; private set;}

    // TODO : Remove the stored GameObjects later if implementing save/load.
    public List<GameObject> WeaponGOs = new();
    private readonly List<BaseWeapon> _weaponScripts = new();
    public IReadOnlyList<BaseWeapon> WeaponScripts => _weaponScripts;


    public UnitWeaponInventory(
                                int weaponsHeld, 
                                int maxWeaponAmount, 
                                ItemPickedUpEventChannel weaponInventoryUpdateEventChannel, 
                                ItemDroppedEventChannel weaponDropEventChannel
                              )
    {
        //WeaponsHeld = weaponsHeld;
        MaxWeaponAmount = maxWeaponAmount;
        _weaponAddedEventChannel = weaponInventoryUpdateEventChannel;
        _weaponDroppedEventChannel = weaponDropEventChannel;
    }

    // Inventory manager does the necessary checks through CanPickupWeapon
    public void AddWeaponToInventory(GameObject weapon, BaseWeapon weaponScript)
    {
        WeaponGOs.Add(weapon);
        _weaponScripts.Add(weaponScript);
        Debug.Log("Added weapon : " + weapon + " to inventory");
        //WeaponsHeld++;
        Debug.Log("Now holding : " + WeaponsHeld + " weapon(s)");

        if (_weaponAddedEventChannel != null)
        {
            // Pass the inventory item SO to UI
            _weaponAddedEventChannel.RaiseEvent(weaponScript.WeaponConfig.InventoryItem);
            Debug.Log("Awesome event raise...");
        }
            
    }

    // Inventory manager does the necessary checks
    public void RemoveWeaponFromInventory(GameObject weapon, BaseWeapon weaponScript)
    {
        WeaponGOs.Remove(weapon);
        _weaponScripts.Remove(weaponScript);
        //WeaponsHeld--;
        Debug.Log("Removed weapon : " + weapon + " from inventory. Now holding : " + WeaponsHeld + " weapon(s).");
        if (_weaponDroppedEventChannel != null)
        {
            // Pass item ID to inventory UI
            _weaponDroppedEventChannel.RaiseEvent(weaponScript.WeaponConfig.InventoryItemID);
        }

    }

    public bool CanPickupWeapon(WeaponConfigSO weaponToPickUp)
    {   
        /*
        if (WeaponsHeld == MaxWeaponAmount)
        {
            Debug.Log("Can't pick up any more weapons.");
            return false;
        }
        */

        
        if (HasWeapon(weaponToPickUp.InventoryItemID))
        {
            Debug.Log("This weapon is already owned. Can't pick it up.");
            return false;
        }
        
        

        return true;
    }

    bool HasWeapon(string itemID)
    {
        return WeaponScripts.Any(w => w.WeaponConfig.InventoryItemID == itemID);
    }

    public int FindNextWeaponWithAmmo(int currentSlot)
    {

        // Check forwards
        for (int i = 1; i < WeaponGOs.Count; i++)
        {
            int nextSlot = (currentSlot + i) % WeaponGOs.Count;
            var weapon = WeaponScripts[nextSlot];
            if (weapon != null && (weapon.AmmoManager.HasAmmo() || weapon.AmmoManager.HasReserveAmmo()))

                return nextSlot;
        }

        // Check backwards
        for (int i = currentSlot; i >= 0; i--)
        {
            int previousSlot = currentSlot - i;
            if (previousSlot < 0)
                break; // no more slots backward

            var weapon = WeaponScripts[previousSlot];
            if (weapon != null && (weapon.AmmoManager.HasAmmo() || weapon.AmmoManager.HasReserveAmmo()))

                return previousSlot;
        }

        return -1; // didn't find any valid weapon
    }

    public bool IsEmpty()
    {
        return WeaponsHeld == 0;
    }

}
