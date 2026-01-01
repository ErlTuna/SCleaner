using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuInventorySection : MonoBehaviour, IMenuSection
{
    public static Action InventoryOpened;
    [SerializeField] InventorySelectedItemDisplay _itemDisplay;
    [SerializeField] List<InventoryUIGroup> _inventoryGroups;
    Queue<InventoryUIGroup> _inventoryUIGroup;
    public bool IsVisible { get; set;}

    void Start()
    {
        _inventoryUIGroup = new(_inventoryGroups);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        PlayerInputManager.Instance.ToggleMouseInput(true);
        foreach (InventoryUIGroup group in _inventoryGroups)
        {
            if (group.IsEmpty() != true)
            {
                UISelector.instance.SetSelected(group.GetDefaultSelected());
                Debug.Log("Gotcha chud! "  + group.GetDefaultSelected());
            }
                
                //EventSystem.current.SetSelectedGameObject(group.GetDefaultSelected());
        }
        IsVisible = true;
    }

    public void Hide()
    {
        PlayerInputManager.Instance.ToggleMouseInput(false);
        UISelector.instance.SetSelected(null);
        IsVisible = false;
        gameObject.SetActive(false);
    }
}
