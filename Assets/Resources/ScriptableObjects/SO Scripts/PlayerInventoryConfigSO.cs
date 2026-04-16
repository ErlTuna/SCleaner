using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Config", menuName = "ScriptableObjects/Component Configs/Inventory Config")]
public class PlayerInventoryConfigSO : ScriptableObject
{
    [Header("Currency")]
    public int MaxCurrency;
    
    [Header("Weapons")]
    public int MaxWeaponAmount;

    public PlayerWeaponConfigSO DefaultWeaponConfig;
    public GameObject EquipmentPrefab;

    [Header("Event Channels")]
    public ItemPickedUpEventChannelSO WeaponAddedEventChannel;
    public ItemDroppedEventChannel WeaponDropEventChannel;
    public ItemPickedUpEventChannelSO ItemPickedUpEventChannel;
    public ItemDroppedEventChannel ItemDroppedEventChannel;
    public IntEventChannelSO CurrencyPickedUpEventChannel;

    //[Header("Currently Unusued")]
    //public VoidEventChannelSO CurrencyChangedEventChannel;

}
