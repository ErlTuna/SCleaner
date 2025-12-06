using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitWeaponInventory
{
    public int WeaponsHeld;
    public int MaxWeaponAmount;
    public List<GameObject> WeaponGOs = new();
    public List<BaseWeapon> WeaponScripts = new();

    public void RemoveWeaponFromInventory(GameObject weapon, BaseWeapon weaponScript)
    {
        WeaponGOs.Remove(weapon);
        WeaponScripts.Remove(weaponScript);
        WeaponsHeld--;
        Debug.Log("Removed weapon : " + weapon + " from inventory. Now holding : " + WeaponsHeld + " weapon(s).");
    }

    public void AddWeaponToInventory(GameObject weapon, BaseWeapon weaponScript)
    {
        WeaponGOs.Add(weapon);
        WeaponScripts.Add(weaponScript);
        Debug.Log("Added weapon : " + weapon + " to inventory");
        WeaponsHeld++;
        Debug.Log("Now holding : " + WeaponsHeld + " weapon(s)");
    }

    public bool CanPickupWeapon(WeaponConfigSO weaponToPickUp)
    {
        if (WeaponsHeld == MaxWeaponAmount)
        {
            Debug.Log("Can't pick up any more weapons.");
            return false;
        }

        foreach (BaseWeapon weaponScript in WeaponScripts)
        {
            if (weaponScript.WeaponConfig.WeaponName == weaponToPickUp.WeaponName)
            {
                Debug.Log("This weapon is already owned. Can't pick it up.");
                return false;
            }
        }

        return true;
    }

    public int FindNextWeaponWithAmmo(int currentSlot)
    {

        // Check forwards
        for (int i = 1; i < WeaponGOs.Count; i++)
        {
            int nextSlot = (currentSlot + i) % WeaponGOs.Count;
            var weapon = WeaponScripts[nextSlot];
            if (weapon != null && weapon.AmmoManager.HasAmmo() || weapon.AmmoManager.HasReserveAmmo())
                return nextSlot;
        }

        // Check backwards
        for (int i = currentSlot; i < 0; i--)
        {
            int previousSlot = currentSlot - i;
            if (previousSlot < 0)
                break; // no more slots backward

            var weapon = WeaponScripts[previousSlot];
            if (weapon != null && weapon.AmmoManager.HasAmmo() || weapon.AmmoManager.HasReserveAmmo())
                return previousSlot;
        }

        return -1; // didn't find any valid weapon
    }

    public bool IsEmpty()
    {
        return WeaponsHeld == 0;
    }

}
