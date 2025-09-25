using System.Collections.Generic;
using UnityEngine;

public interface IUnitInventoryData : IUnitRuntimeData, IConfigurable<UnitInventoryConfigSO>, IAutoConfigurable
{
    public List<GameObject> WeaponPrefabs { get; set; }
    public List<GameObject> WeaponGOs { get; set; }
    public List<GameObject> EquipmentPrefabs { get; set; }
    public List<GameObject> EquipmentGOs { get; set; }
    public List<BaseEquipment> EquipmentScripts { get; set; }
    public int OwnedCurrency { get; set; }
    public int MaxCurrency { get; set; }
    public int WeaponsHeld { get; set; }
    public int EquipmentsHeld { get; set; }

    public UnitWeaponInventory WeaponInventory { get; set; }
    public UnitEquipmentInventory EquipmentInventory { get; set; }
    public UnitCurrencyInventory CurrencyInventory { get; set; }

}
