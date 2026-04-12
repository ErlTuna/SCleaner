
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance { get; private set; }

    [SerializeField] GameObject _lootChestPrefab;
    [SerializeField] ItemDatabaseSO _itemDB;
    [SerializeField] List<ItemDatabaseEntry> _remainingItems;
    [SerializeField] List<ItemDatabaseEntry> _repeatableDropPool;
    [SerializeField] List<ItemDatabaseEntry> _uniqueDropPool;
    [SerializeField] int _lootChestDropChanceMin = 0;
    [SerializeField] int _currentLootChestDropChance = 0;
    [SerializeField] int _lootChestDropChanceMax = 10;
    [SerializeField] int _lootChestDropChanceIncreasePerFail = 5;
    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroying something...");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach (ItemDatabaseEntry entry in _itemDB.DatabaseEntries)
        {
            if (entry.LootBehaviour == LootBehavior.REPEATABLE)
            {
                _repeatableDropPool.Add(entry);
            }
            
            else
            {
                _uniqueDropPool.Add(entry);
            }
        }
    }

    public void TrySpawnLootChest(Vector3 location)
    {
        
        if (Random.Range(_lootChestDropChanceMin, _lootChestDropChanceMax) > _currentLootChestDropChance)
        {
            Debug.Log("Couldn't spawn loot chest. Bad luck!");
            _currentLootChestDropChance += _lootChestDropChanceIncreasePerFail;
            return;   
        }

        GameObject lootChestGO = Instantiate(_lootChestPrefab, location, Quaternion.identity);

        LootChestController lootChestController = lootChestGO.GetComponent<LootChestController>();
        lootChestController.AssignLootItem(GetRandomLootItem());

        _currentLootChestDropChance = _lootChestDropChanceMin;
    }

    // The GO is returned inactive.
    public GameObject GetRandomLootItem()
    {
        if (_uniqueDropPool.Count == 0) return null;

        int randomIndex = Random.Range(0, _uniqueDropPool.Count);
        ItemDatabaseEntry entry = _uniqueDropPool[randomIndex];
        GameObject lootGO = Instantiate(entry.PickupConfig.PickupPrefab, transform.position, Quaternion.identity);
        lootGO.SetActive(false);

        //if (entry.PickupConfig is WeaponPickupSO == false)
        //{
            lootGO.GetComponent<ItemPickup>().SetPickupConfig(entry.PickupConfig);
        //}

        // Weapon pick ups all have their own Pickup Prefabs (as they use an extension of ItemPickup MB) so they come with their own weapon configs
        // As such, generic ItemPickup needs to have its configuration injected.
        //if (entry.PickupConfig is WeaponPickupSO == false)
        //{
            //lootGO.GetComponent<ItemPickup>().SetPickupConfig(entry.PickupConfig);
        //}
        
        //lootGO.SetActive(true);


        return lootGO;
        
    }

    // For Health, Ammo and Shield
    public GameObject GetRandomPickupItem()
    {
        if (_repeatableDropPool.Count == 0) return null;



        int randomIndex = Random.Range(0, _repeatableDropPool.Count);
        ItemDatabaseEntry entry = _repeatableDropPool[randomIndex];
        GameObject lootGO = Instantiate(entry.PickupConfig.PickupPrefab, transform.position, Quaternion.identity);
        lootGO.SetActive(false);

        // Weapon pick ups all have their own Pickup Prefabs (as they use an extension of ItemPickup MB)
        // As such, generic ItemPickup needs to have its configuration injected.
        //if (entry.PickupConfig is WeaponPickupSO == false)
        {
            lootGO.GetComponent<ItemPickup>().SetPickupConfig(entry.PickupConfig);
        }
        
        //lootGO.SetActive(true);


        return lootGO;
        
    }


    void TrackEnemyDefeatedCount()
    {
        
    }




}
