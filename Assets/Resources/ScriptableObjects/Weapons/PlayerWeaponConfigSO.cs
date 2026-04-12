using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeaponConfigSO", menuName = "ScriptableObjects/Weapons/Player Weapon Config")]
public class PlayerWeaponConfigSO : WeaponConfigSO
{
    // ItemID is a shortcut to InventoryItem.ItemID
    public string InventoryItemGUID => InventoryItemDefinition.ItemID;

    [Header("Inventory Item Data")]
    public InventoryItemDefinitionSO InventoryItemDefinition;

    [Header("Pickup Definition")]
    public ItemSO PickupConfig;
    public GameObject PickupPrefab;
}
