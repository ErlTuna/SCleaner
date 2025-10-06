using SerializeReferenceEditor;
using UnityEngine;
public class PlayerEquipmentManager_v2 : MonoBehaviour
{
    [SerializeField] GameObject _currentEquipmentGO;
    [SerializeField] BaseEquipment _currentEquipmentScript;
    [SerializeField] Transform _equipmentSpawnPoint;
    //[SerializeField] Transform _parentTransform;

    void OnEnable()
    {
        PlayerInventoryEvents.OnEquipmentReady += ReceiveEquipment;

    }

    void OnDisable()
    {
        PlayerInventoryEvents.OnEquipmentReady -= ReceiveEquipment;

    }

    void Update()
    {

        if (PlayerInputManager.instance.EquipmentInput && _currentEquipmentScript.CanUseEquipment())
        {
            Debug.Log("Deploying equipment");
            DeployEquipment();
        }

    }

    void ChangeEquipment()
    {
        Debug.Log("called");
        //IEquipment.InvokeEquipmentChangeEvent(_currentEquipmentScript.EquipmentInfo);
    }

    void DeployEquipment()
    {
        _currentEquipmentScript.InitializeDeployment(transform.localRotation, transform.right, transform, _equipmentSpawnPoint.position);
        _currentEquipmentScript.OnDeployment();


        // StartCoroutine(_currentEquipmentScript.TriggerAbilityAfterTime());
        //--_currentEquipmentScript.EquipmentInfo.count;
        //IEquipment.InvokeEquipmentUseEvent(_currentEquipmentScript.EquipmentInfo);
    }


    public void ReceiveEquipment(GameObject equipment, BaseEquipment equipmentScript)
    {
        _currentEquipmentGO = equipment;
        _currentEquipmentScript = equipmentScript;
        //_currentWeaponGO.SetActive(true);
    }


}