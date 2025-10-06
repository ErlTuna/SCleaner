using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Config", menuName = "ScriptableObjects/Component Configs/Inventory Config")]
public class UnitInventoryConfigSO : ScriptableObject
{
    [Header("Currency")]
    public int OwnedCurrency;
    public int MaxCurrency;

    [Header("Weapons")]
    public int MaxWeaponAmount;
    public List<GameObject> WeaponPrefabs = new();
    public List<GameObject> EquipmentPrefabs = new();

}
