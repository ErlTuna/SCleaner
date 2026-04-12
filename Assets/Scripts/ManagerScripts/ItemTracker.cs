using System.Collections.Generic;
using UnityEngine;

public class ItemTracker : MonoBehaviour
{
    public static ItemTracker Instance { get; private set; }
    readonly HashSet<string> _consumedItems = new();


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroying something...");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool HasBeenConsumed(string itemID) => _consumedItems.Contains(itemID);

    public void MarkAsConsumed(string itemID)
    {
        if (_consumedItems.Contains(itemID))
            return;
        
        Debug.Log($"Marked item ${itemID} as consumed.");
        _consumedItems.Add(itemID);
    }


    /*
    public GameObject GetRandomItem()
    {
        if (_uniqueDropPool.Count == 0) return null;

        int randomIndex = Random.Range(0, _uniqueDropPool.Count);
        ItemDatabaseEntry entry = _uniqueDropPool.ElementAt(randomIndex);
        GameObject pickupGO = Instantiate(entry.PickupPrefab, transform.position, Quaternion.identity);

        

        pickupGO.SetActive(false);

        // Weapon pick ups all have their own Pickup Prefabs (as they use an extension of ItemPickup MB)
        // As such, generic ItemPickup needs to have its configuration injected.
        if (entry.PickupConfig is WeaponPickupSO == false)
        {
            pickupGO.GetComponent<ItemPickup>().SetPickupConfig(entry.PickupConfig);
        }
        
        pickupGO.SetActive(true);


        return pickupGO;
    }
    */
}
