using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This is put on a prefab that acts as the visual representation of an inventory item to display it in the Inventory screen.
public class InventoryEntry : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    public InventoryEntry Up,Down,Left,Right;
    public RectTransform RectTransform;
    [SerializeField] Image _selectionHighlight;
    [SerializeField] Image _imageComponent;
    [SerializeField] InventoryItem _inventoryItem;
    public InventoryItem InventoryItem => _inventoryItem;


    void Awake()
    {
        if (_inventoryItem != null)
            _imageComponent.sprite = _inventoryItem.ItemData.ItemIcon;
    }

    public void Initialize(InventoryItem inventoryItem)
    {
        _inventoryItem = inventoryItem;
        _imageComponent.sprite = inventoryItem.ItemData.ItemIcon;
        Debug.Log("Initialized inventory entry...");
    }

    // Unity does not automatically select a GameObject with Selectable on Pointer Click.
    // As such, I do it here.
    public void OnPointerClick(PointerEventData eventData)
    {
        UISelector.instance.SetSelected(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (_selectionHighlight != null)
        {
            _selectionHighlight.enabled = true;
            InventorySelectedItemDisplay.Instance.OnItemSelected(_inventoryItem);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (_selectionHighlight != null)
        {
            _selectionHighlight.enabled = false;
        }
    }
}

