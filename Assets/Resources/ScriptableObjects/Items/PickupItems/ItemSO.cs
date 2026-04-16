using System;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Pickups/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Pickup Metadata")]
    public ItemPickupDefinitionSO PickupDefinition; // name, icon, description, SFX

    [Header("Pickup Inventory Data")]
    public InventoryItemDefinitionSO InventoryDefinition; // inventory name, inventory icon etc

    [Header("Pickup Prefab")]
    public GameObject PickupPrefab;

    [Header("Pickup Conditions")]
    [Tooltip("With no conditions, an item will always be pickable.")]
    public PickupConditionSO[] GeneralConditions; // rules for whether it can be picked up

    [Header("Pickup Behaviours")]
    [Tooltip("If true, the behaviours will sort themselves in ascending order.")]
    [SerializeField] bool autoReorder;
    public PickupBehaviorEntry[] Behaviors;

    [Header("Optional Scale Overrides")]
    [Range(.1f, 2f)] public float xScaleOverride = 1f;
    [Range(.1f, 2f)] public float yScaleOverride = 1f;
    
    [SerializeField] [HideInInspector] string _itemId;
    public string ItemID => _itemId; // unique per pickup instance

    public void SortBehaviorsByOrder(bool autoReorder = true)
    {
        if (Behaviors == null || Behaviors.Length <= 1) return;
        if (!autoReorder) return;
        Array.Sort(Behaviors, (a, b) => a.Order.CompareTo(b.Order));
    }

    void OnEnable()
    {
        #if UNITY_EDITOR
        if (autoReorder)
        {
            SortBehaviorsByOrder();
            EditorUtility.SetDirty(this);
        }
        #endif
    }

    #if UNITY_EDITOR
    void OnValidate()
    {
        if (autoReorder)
        {
            SortBehaviorsByOrder();
            EditorUtility.SetDirty(this);
        }

        if (string.IsNullOrEmpty(_itemId))
        {
            _itemId = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
        }
    }

    [ContextMenu("Regenerate Item ID")]

    public void RegenerateId()
    {
        _itemId = Guid.NewGuid().ToString();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
    #endif


    public virtual IPickupPayload CreatePayload()
    { 
        return null; 
    }

    
    #if UNITY_EDITOR
    [ContextMenu("PRINT DEBUG")]
    void PrintDebug()
    {
        Debug.Log($"ItemSO: {name}");
        Debug.Log($"Instance ID: {GetInstanceID()}");
        Debug.Log($"ItemID: {_itemId}");
        Debug.Log($"SourceItem: {InventoryDefinition.SourceItem}");
        Debug.Log($"ItemSO name: {InventoryDefinition.name}");
        Debug.Log($"ItemSO path: {UnityEditor.AssetDatabase.GetAssetPath(InventoryDefinition)}");
    }
    #endif
}   
