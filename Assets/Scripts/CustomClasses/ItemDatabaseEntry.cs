using System;
using UnityEngine;

[Serializable]
public class ItemDatabaseEntry
{
    public string ItemName => PickupConfig.PickupDefinition.PickUpName;
    public GameObject PickupPrefab;
    public ItemSO PickupConfig;
    public LootBehavior LootBehaviour;
}
