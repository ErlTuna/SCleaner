using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerEquipmentManager : MonoBehaviour
{
    GameObject _equipmentPrefab;
    Transform _grenadeSpawnPoint;
    Transform _parentTransform;
    public bool isThereActiveEquipment = false;
    public int _equipmentCount = 5;
    IEquipment _equipment;
    
    void OnEnable(){
        if(_equipment != null)
            _equipment.OnAbilityEnd += ResetEquipmentStatus;

        _equipment.DisableScript();
        _equipmentPrefab.SetActive(false);
    }

    void OnDisable(){
        _equipment.OnAbilityEnd -= ResetEquipmentStatus;
    }

    void Awake(){
        if(!_equipmentPrefab)
          _equipmentPrefab = Instantiate(Resources.Load("WeaponPrefabs/GrenadePrefab") as GameObject);
        else
            _equipmentPrefab = Instantiate(_equipmentPrefab);
        
        if(!_grenadeSpawnPoint)
            _grenadeSpawnPoint = GameObject.FindGameObjectWithTag("GrenadeSpawnPoint").transform;

        _equipment = _equipmentPrefab.GetComponent<IEquipment>();

    }
    void Start()
    {
        _parentTransform = transform;
        _equipmentPrefab.transform.parent = _parentTransform;
        _equipmentPrefab.transform.position = _grenadeSpawnPoint.position;
        
    }

    void Update()
    {
        if (PlayerInputManager.instance.EquipmentInput && _equipmentCount != 0 && !isThereActiveEquipment){
            DeployEquipment();
        }
    }
    
    public void DeployEquipment()
    {

        _equipmentPrefab.SetActive(true);
        _equipment.SetupEquipmentParameters(transform.localRotation, transform.right, _parentTransform, _grenadeSpawnPoint);
        _equipmentPrefab.transform.parent = null;
        StartCoroutine(_equipment.TriggerAbilityAfterTime());
        isThereActiveEquipment = true;
        --_equipmentCount;

    }

    void ResetEquipmentStatus(){
        isThereActiveEquipment = false;
    }

}
