using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



// With the interface, if needed, these groups can be gathered in a manager
public class InventoryUIGroup : MonoBehaviour, IInventoryUIGroup
{
    [SerializeField] GameObject _inventoryEntryPrefab;
    [SerializeField] Transform _itemContainer;
    [SerializeField] ItemPickedUpEventChannelSO _itemAddedEventChannel;
    [SerializeField] ItemDroppedEventChannel _itemDroppedEventChannel;
    readonly Dictionary<string, GameObject> _groupItemsDict = new();
    readonly List<GameObject> _orderedItems = new();
    [SerializeField] WrapLayout _selfWrapLayout;

    void Awake()
    {
        _itemAddedEventChannel.OnEventRaised += AddItem;
        _itemDroppedEventChannel.OnEventRaised += RemoveItem;
    }

    void OnDestroy()
    {
        _itemAddedEventChannel.OnEventRaised -= AddItem;
        _itemDroppedEventChannel.OnEventRaised -= RemoveItem;
    }

    public void AddItem(InventoryItemDefinitionSO itemData)
    {
        // Create the entry
        GameObject entryGO = Instantiate(_inventoryEntryPrefab, _itemContainer);
        InventoryEntry entry = entryGO.GetComponent<InventoryEntry>();

        // Create the POCO wrapper
        InventoryItem inventoryItem = new(itemData);

        // Initialize entry using the POCO
        entry.Initialize(inventoryItem);


        _orderedItems.Add(entryGO);
        _groupItemsDict.Add(itemData.ItemID, entryGO);

        _selfWrapLayout.Rebuild();
    }

    public void RemoveItem(string itemID)
    {
        if(_groupItemsDict.TryGetValue(itemID, out GameObject entry))
        {
            _orderedItems.Remove(entry);
            _groupItemsDict.Remove(itemID);
            Destroy(entry);

            if (_groupItemsDict.Count == 0)
            {
                InventorySelectedItemDisplay.Instance.Reset();
            }
                
        }

        _selfWrapLayout.Rebuild();
    }

    public bool IsEmpty()
    {
        return _groupItemsDict.Count == 0;
    }

    public GameObject GetDefaultSelected()
    {
        if (_orderedItems[0] != null)
            Debug.Log("What is ordered items 0 : " + _orderedItems[0]);

        return _orderedItems.Count > 0 ? _orderedItems[0] : null;
    }

}

public interface IInventoryUIGroup
{
    void AddItem(InventoryItemDefinitionSO itemData);
    void RemoveItem(string itemID);
    GameObject GetDefaultSelected();
}

