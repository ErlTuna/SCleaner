using System;
using UnityEngine;

[Serializable]
public class UnitEquipmentInventory
{
    public GameObject EquipmentGO;
    public BaseEquipment EquipmentScript;
    public void AddEquipment(GameObject equipmentGO, BaseEquipment equipmentScript)
    {
        EquipmentGO = equipmentGO;
        EquipmentScript = equipmentScript;
    }
}
