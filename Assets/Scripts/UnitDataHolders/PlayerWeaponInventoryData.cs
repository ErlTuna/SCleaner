using System;
using System.Collections.Generic;

[Serializable]
public class PlayerWeaponInventoryData
{   
    public List<WeaponInventoryEntry> Weapons = new();
    public int MaxWeaponAmount;

}