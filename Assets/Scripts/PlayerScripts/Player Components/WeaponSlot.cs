using System;
using UnityEngine;

/// <summary>
/// Wrapper class used by PlayerInventoryManager.
/// Caches the essential data that defines a weapon:
/// its GameObject and the associated BaseWeapon script.
/// </summary>
/// 
[Serializable]
public class WeaponSlot
{
    public GameObject Weapon;
    public PlayerWeapon Script;

    /// <summary>
    /// Initializes a new weapon slot with the specified weapon and script.
    /// </summary>
    /// <param name="weapon">The weapon GameObject.</param>
    /// <param name="script">The associated BaseWeapon script.</param>
    public WeaponSlot(GameObject weapon, PlayerWeapon script)
    {
        Weapon = weapon;
        Script = script;
        //Debug.Log($"A weapon slot was created with game object {weapon} and script {script}");
    }

    /// <summary>
    /// Clears the weapon slot by setting its references to null.
    /// Should be used before the WeaponSlot is removed from wherever.
    /// </summary>

    public void ClearSlot()
    {
        Weapon = null;
        Script = null;
    }

}
