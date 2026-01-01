using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySelectedItemDisplay : MonoBehaviour
{

    [SerializeField] TMP_Text _itemName;
    [SerializeField] Image _itemIcon;
    [SerializeField] TMP_Text _itemType;
    [SerializeField] TMP_Text _itemDescription;
    public static InventorySelectedItemDisplay Instance {get; private set;}
    InventoryItem _previousSelected;

    void OnEnable()
    {
        Reset();
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
    }

    public void OnItemSelected(InventoryItem selected)
    {
        Debug.Log("An item is selected.");
            if (selected != null && selected != _previousSelected)
            {
                _previousSelected = selected;

                //InventoryItemDefinitionSO itemData = entry.InventoryItem.ItemData;

                if (_itemName != null) _itemName.text = selected.ItemData.ItemName;

                if (_itemIcon != null)
                {
                    _itemIcon.sprite = selected.ItemData.ItemIcon;
                    _itemIcon.color = Color.white;
                    //_itemIcon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,selected.ItemData.ItemIcon.rect.width);
                    //_itemIcon.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,selected.ItemData.ItemIcon.rect.height);
                } 

                if (_itemType != null) _itemType.text = selected.ItemData.ItemType.ToString();

                if (_itemDescription != null) _itemDescription.text = selected.ItemData.ItemDescription;

                Debug.Log("Showing item info.");
            }
    }

    public void Reset()
    {
        _itemName.text = "";
        _itemIcon.color = new Color(1, 1, 1, 0);
        _itemType.text = "";
        _itemDescription.text = "";
    }


}
