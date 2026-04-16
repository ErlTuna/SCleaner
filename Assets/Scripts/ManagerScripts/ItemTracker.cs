using System.Collections.Generic;
using UnityEngine;

public class ItemTracker : MonoBehaviour
{
    public static ItemTracker Instance { get; private set; }
    HashSet<string> _consumedItems;


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

    void OnEnable()
    {
        GameManager.OnLevelLoaded += Initialize;
    }

    void OnDisable()
    {
        GameManager.OnLevelLoaded -= Initialize;
    }



    public bool HasBeenConsumed(string itemID) => _consumedItems.Contains(itemID);

    public void MarkAsConsumed(string itemID)
    {
        if (_consumedItems.Contains(itemID))
            return;
        
        Debug.Log($"Marked item {itemID} as consumed.");
        _consumedItems.Add(itemID);
    }

    void Initialize()
    {
        _consumedItems = new();
    }
    
}
