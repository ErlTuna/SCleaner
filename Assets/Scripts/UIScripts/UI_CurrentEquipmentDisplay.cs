using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CurrentEquipmentDisplay : MonoBehaviour
{
    [SerializeField] Image _imageComponent;
    [SerializeField] TextMeshProUGUI _equipmentCountText;

    void OnEnable()
    {
        IEquipment.OnEquipmentChanged += UpdateEquipmentDisplay;
        IEquipment.OnEquipmentUsed += UpdateEquipmentCount;
    }

    void OnDisable()
    {
        IEquipment.OnEquipmentChanged -= UpdateEquipmentDisplay;
        IEquipment.OnEquipmentUsed -= UpdateEquipmentCount;
    }

    void UpdateEquipmentDisplay(EquipmentSO equipmentSO)
    {
        Debug.Log("awesome");
        if (equipmentSO)
            _imageComponent.sprite = equipmentSO.sprite;
    }

    void UpdateEquipmentCount(EquipmentSO equipmentSO)
    {
        _equipmentCountText.SetText(equipmentSO.count.ToString());
    }

}
