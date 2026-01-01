using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_CurrentEquipmentDisplay : MonoBehaviour
{
    [SerializeField] Image _imageComponent;
    [SerializeField] TextMeshProUGUI _equipmentCountText;

    void OnEnable()
    {
        //PlayerInventoryEvents.OnEquipmentSwitchUIUpdate += UpdateEquipmentDisplay;
        //IEquipment.OnEquipmentChanged += UpdateEquipmentDisplay;
        //IEquipment.OnEquipmentUsed += UpdateEquipmentCount;
    }

    void OnDisable()
    {
        //PlayerInventoryEvents.OnEquipmentSwitchUIUpdate -= UpdateEquipmentDisplay;
        //IEquipment.OnEquipmentChanged -= UpdateEquipmentDisplay;
        //IEquipment.OnEquipmentUsed -= UpdateEquipmentCount;
    }

    
    void UpdateEquipmentDisplay(Sprite equipmentSprite, EquipmentData equipmentData)
    {
        Debug.Log("Greetings...");
        if (equipmentSprite != null && equipmentData != null)
        {
            Debug.Log("Interesting...");
            _imageComponent.sprite = equipmentSprite;
            _imageComponent.color = Color.white;
        }        
    }

}
