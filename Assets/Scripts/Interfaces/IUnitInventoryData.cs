using System.Collections.Generic;
using UnityEngine;

public interface IUnitInventoryData : IUnitRuntimeData, IConfigurable<UnitInventoryConfigSO>, IAutoConfigurable
{
    public List<GameObject> weaponPrefabs { get; set; }
    public List<OLD_BaseWeapon> WeaponScripts { get; set; }
    public List<GameObject> WeaponGOs { get; set; }
    public int OwnedCurrency { get; set; }
    public int MaxCurrency { get; set; }
    public int WeaponsHeld { get; set; }
}
