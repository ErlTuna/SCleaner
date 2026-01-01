using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Config", menuName = "ScriptableObjects/Component Configs/Inventory Config")]
public class UnitInventoryConfigSO : ScriptableObject
{
    [Header("Currency")]
    public int MaxCurrency;

    [Header("Weapons")]
    public int MaxWeaponAmount;
    public List<GameObject> WeaponPrefabs = new();
    public GameObject EquipmentPrefab;

    [Header("Event Channels")]
    public ItemPickedUpEventChannel WeaponAddedEventChannel;
    public ItemDroppedEventChannel WeaponDropEventChannel;
    public ItemPickedUpEventChannel EquipmentPickedUpChannel;
    public ItemDroppedEventChannel EquipmentDroppedChannel;

    [Header("Currently Unusued")]
    public VoidEventChannelSO CurrencyChangedEventChannel;

}
