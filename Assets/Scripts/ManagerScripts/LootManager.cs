
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
    bool _isLootChestDropEnabled = true;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroying something...");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        /*
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
        */
    }

    void OnEnable()
    {
        GameManager.OnLevelLoaded += Initialize;
    }

    void OnDisable()
    {
        GameManager.OnLevelLoaded -= Initialize;
    }


    public void TrySpawnLootChest(Vector3 location)
    {
        
        if (_isLootChestDropEnabled == false)  
            return;

        if (Random.Range(_lootChestDropChanceMin, _lootChestDropChanceMax) > _currentLootChestDropChance)
        {
            //Debug.Log("Couldn't spawn loot chest. Bad luck!");
            _currentLootChestDropChance += _lootChestDropChanceIncreasePerFail;
            return;
        }

        if (TryGetRandomLootItem(out ItemDatabaseEntry entry) == false) return;
        
        GameObject lootChestGO = Instantiate(_lootChestPrefab, location, Quaternion.identity);
        LootChestController lootChestController = lootChestGO.GetComponent<LootChestController>();


        GameObject lootGO = Instantiate( entry.PickupConfig.PickupPrefab, transform.position, Quaternion.identity);
        lootGO.SetActive(false);

        if (lootGO.TryGetComponent<ItemPickup>(out var pickup))
            pickup.SetPickupConfig(entry.PickupConfig);
        

        lootChestController.AssignLootItem(lootGO);
        _currentLootChestDropChance = _lootChestDropChanceMin;
    }

    // For Health, Ammo and Shield
    public GameObject GetRandomPickupItem(Vector3 location)
    {
        if (_repeatableDropPool.Count == 0) return null;

        int randomIndex = Random.Range(0, _repeatableDropPool.Count);
        ItemDatabaseEntry entry = _repeatableDropPool[randomIndex];
        Debug.Log("Pulled from repeatable pool : " + entry.ItemName);
        Vector3 offSet = new(.5f, 0f);
        GameObject lootGO = Instantiate(entry.PickupConfig.PickupPrefab, location + offSet, Quaternion.identity);
        //lootGO.SetActive(false);

        // Weapon pick ups all have their own Pickup Prefabs (as they use an extension of ItemPickup MB)
        // As such, generic ItemPickup needs to have its configuration injected.
        //if (entry.PickupConfig is WeaponPickupSO == false)
        {
            lootGO.GetComponent<ItemPickup>().SetPickupConfig(entry.PickupConfig);
        }
        
        //lootGO.SetActive(true);


        return lootGO;
        
    }

    

    public bool TryGetRandomLootItem(out ItemDatabaseEntry entry)
    {
        entry = null;

        if (_uniqueDropPool.Count == 0)
            return false;
        
        int randomIndex = Random.Range(0, _uniqueDropPool.Count);
        entry = _uniqueDropPool[randomIndex];

        _uniqueDropPool.RemoveAt(randomIndex);

        return true;
    }

    void Initialize()
    {
        _repeatableDropPool.Clear();
        _uniqueDropPool.Clear();

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

    public void EnableLootDrops(bool enable)
    {
        _isLootChestDropEnabled = enable;
        Debug.Log("Loot chest drops :" + enable); 
    }

}
