using SerializeReferenceEditor;
using UnityEngine;
public class PlayerEquipmentManager : MonoBehaviour
{
    [SerializeField] GameObject _currentEquipmentGO;
    [SerializeField] BaseEquipment _currentEquipmentScript;
    [SerializeField] Transform _equipmentSpawnPoint;
    //[SerializeField] Transform _parentTransform;

    void OnEnable()
    {
        //PlayerInventoryEvents.OnEquipmentReady += ReceiveEquipment;
    }

    void OnDisable()
    {
        //PlayerInventoryEvents.OnEquipmentReady -= ReceiveEquipment;

    }

    void Update()
    {

        if (PlayerInputManager.Instance.EquipmentInput && _currentEquipmentScript.CanUseEquipment())
        {
            Debug.Log("Deploying equipment");
            UseEquipment();
        }

    }

    void UseEquipment()
    {
        _currentEquipmentScript.InitializeDeployment(transform.localRotation, transform.right, transform, _equipmentSpawnPoint.position);
        _currentEquipmentScript.OnUse();
    }


    public void ReceiveEquipment(GameObject equipment, BaseEquipment equipmentScript)
    {
        _currentEquipmentGO = equipment;
        _currentEquipmentScript = equipmentScript;
    }

}