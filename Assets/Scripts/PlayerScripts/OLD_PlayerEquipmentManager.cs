using System;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class OLD_PlayerEquipmentManager : MonoBehaviour
{
    /*
    GameObject _equipmentPrefab;
    Transform _grenadeSpawnPoint;
    Transform _parentTransform;
    public bool isThereActiveEquipment = false;
    public int _equipmentCount = 5;
    IEquipment _currentEquipmentScript;

    void OnEnable()
    {
        if (_currentEquipmentScript != null)
            _currentEquipmentScript.OnAbilityEnd += ResetEquipmentStatus;

        //_currentEquipmentScript.DisableScript();
        //_equipmentPrefab.SetActive(false);
        
        
    }

    void OnDisable(){
        //_currentEquipmentScript.OnAbilityEnd -= ResetEquipmentStatus;
    }

    void Awake(){
        /*
        if (!_equipmentPrefab)
            _equipmentPrefab = Instantiate(Resources.Load("WeaponPrefabs/GrenadePrefab") as GameObject);
        else
            _equipmentPrefab = Instantiate(_equipmentPrefab);
        
        if(!_grenadeSpawnPoint)
            _grenadeSpawnPoint = GameObject.FindGameObjectWithTag("GrenadeSpawnPoint").transform;

        _currentEquipmentScript = _equipmentPrefab.GetComponent<IEquipment>();
        

    }
    void Start()
    {
        //_parentTransform = transform;
        //_equipmentPrefab.transform.parent = _parentTransform;
        //_equipmentPrefab.transform.position = _grenadeSpawnPoint.position;
    }

    void Update()
    {
        /*
        if (PlayerInputManager.instance.EquipmentInput && _currentEquipmentScript.EquipmentInfo.count != 0 && !isThereActiveEquipment)
        {
            ChangeEquipment();
            DeployEquipment();
        }
        
    }

    void ChangeEquipment()
    {
        Debug.Log("called");
        IEquipment.InvokeEquipmentChangeEvent(_currentEquipmentScript.EquipmentInfo);
    }
    
    void DeployEquipment()
    {
        _equipmentPrefab.SetActive(true);
        _currentEquipmentScript.SetupEquipmentParameters(transform.localRotation, transform.right, _parentTransform, _grenadeSpawnPoint);
        _equipmentPrefab.transform.parent = null;
        StartCoroutine(_currentEquipmentScript.TriggerAbilityAfterTime());
        isThereActiveEquipment = true;
        --_currentEquipmentScript.EquipmentInfo.count;
        IEquipment.InvokeEquipmentUseEvent(_currentEquipmentScript.EquipmentInfo);
    }

    void ResetEquipmentStatus() {
    isThereActiveEquipment = false;
    }
    
    */

}
