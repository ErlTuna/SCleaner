using System;
using UnityEngine;

/// <summary>
/// Wrapper class used by PlayerInventoryManager.
/// Caches the essential data that defines a Equipment:
/// its GameObject and the associated BaseEquipment script.
/// </summary>
/// 
[Serializable]
public class EquipmentSlot
{
    public GameObject Equipment;
    public BaseEquipment Script;

    /// <summary>
    /// Initializes a new Equipment slot with the specified Equipment and script.
    /// </summary>
    /// <param name="Equipment">The Equipment GameObject.</param>
    /// <param name="script">The associated BaseEquipment script.</param>
    public EquipmentSlot(GameObject equipment, BaseEquipment script)
    {
        Equipment = equipment;
        Script = script;
        //Debug.Log($"A Equipment slot was created with game object {Equipment} and script {script}");
    }

    /// <summary>
    /// Clears the Equipment slot by setting its references to null.
    /// Should be used before the EquipmentSlot is removed from wherever.
    /// </summary>

    public void ClearSlot()
    {
        Equipment = null;
        Script = null;
    }

}
