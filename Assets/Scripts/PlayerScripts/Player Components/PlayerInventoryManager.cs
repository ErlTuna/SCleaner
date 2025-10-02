using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using SerializeReferenceEditor;

public class PlayerInventoryManager : MonoBehaviour
{
    Unit _owner;
    UnitInventoryData _inventoryData;
    [SerializeField] Transform _weaponParent;
    [SerializeField] Transform _weaponPosition;
    [SerializeField] Transform _equipmentPosition;
    [SerializeReference] List<WeaponSlot> _weaponSlots = new();
    WeaponSlot _currentlyUsedWeaponSlot;
    GameObject _currentEquipment;
    BaseEquipment _currentEquipmentScript;
    int _currentlyUsedEquipmentSlot = 0;
    int _currentlyUsedWeaponSlotIndex = -1;
    //bool _isHandlingEmptyWeapon = false;

    /*
    void OnEnable()
    {
        //WeaponEvents.OnWeaponOutOfAmmoEvent += OutOfAmmoAutoSwitch;
    }

    void OnDisable()
    {
        //WeaponEvents.OnWeaponOutOfAmmoEvent -= OutOfAmmoAutoSwitch;
    }
    */

    void Start()
    {
        SETTINGS.AutoSwitchToPickedUpWeapon = true;
    }

    void Update()
    {
        if (PlayerInputManager.instance.WeaponDropInput)
            DropCurrentWeapon();

        if (PlayerInputManager.instance.WeaponSwitchInput != -1)
            SwitchToWeaponSlot(PlayerInputManager.instance.WeaponSwitchInput);
        
        /*
        else if (PlayerInputManager.instance.WeaponSwitchInput)
            SwitchToWeaponSlot(0);
        else if (PlayerInputManager.instance.SecondWeaponInput)
            SwitchToWeaponSlot(1);
        else if (PlayerInputManager.instance.ThirdWeaponInput)
            SwitchToWeaponSlot(2);
        else if (PlayerInputManager.instance.FourthWeaponInput)
            SwitchToWeaponSlot(3);
        */
    }

    public void InitializeWithData(Unit owner, UnitInventoryData inventoryData)
    {
        _owner = owner;
        _inventoryData = inventoryData;


        PrepareWeapons();
        //SetDefaultWeapon();
        PrepareEquipments();
        SetDefaultEquipment();
    }

    void PrepareWeapons()
    {
        int currentSlot = 0;
        foreach (GameObject weaponPrefab in _inventoryData.InventoryConfig.WeaponPrefabs)
        {
            GameObject weaponGO = Instantiate(weaponPrefab);
            weaponGO.transform.parent = _weaponParent;
            weaponGO.transform.SetPositionAndRotation(_weaponPosition.position, _weaponPosition.rotation);

            BaseWeapon weaponScript = weaponGO.GetComponent<BaseWeapon>();
            weaponScript.InitializeWithConfig();

            weaponGO.transform.localPosition += weaponScript.WeaponConfig.offset;

            if (weaponScript != null)
            {
                _inventoryData.WeaponInventory.WeaponGOs.Add(weaponGO);
                _inventoryData.WeaponInventory.WeaponScripts.Add(weaponScript);

                WeaponSlot slot = new(weaponGO, weaponScript);
                _weaponSlots.Add(slot);
                weaponGO.SetActive(false);
            }

            currentSlot++;
        }
    }

    void PrepareEquipments()
    {
        foreach (GameObject equipmentPrefab in _inventoryData.InventoryConfig.EquipmentPrefabs)
        {
            GameObject equipmentGO = Instantiate(equipmentPrefab);
            equipmentGO.transform.parent = _weaponParent;
            equipmentGO.transform.position = _equipmentPosition.position;

            BaseEquipment equipmentScript = equipmentGO.GetComponent<BaseEquipment>();
            equipmentScript.InitializeWithData();


            if (equipmentScript != null)
            {
                _inventoryData.EquipmentInventory.EquipmentGOs.Add(equipmentGO);
                _inventoryData.EquipmentInventory.EquipmentScripts.Add(equipmentScript);
                equipmentGO.SetActive(false);
            }
        }
    }

    void SwitchToWeaponSlot(int slotToSwitchTo)
    {
        Debug.Log("Attempting to switch to slot : " + slotToSwitchTo);
        //if (slotToSwitchTo >= _inventoryData.WeaponInventory.WeaponGOs.Count)

        if (slotToSwitchTo >= _weaponSlots.Count)
        {
            Debug.Log("Slot to switch to : " + slotToSwitchTo + " is out of bounds for the weapon slots list.");
            return;
        }

        
        if (_weaponSlots[slotToSwitchTo] != null && slotToSwitchTo != _currentlyUsedWeaponSlotIndex)
        {
            WeaponSlotChange(_currentlyUsedWeaponSlotIndex, slotToSwitchTo);
            Debug.Log("Switched to weapon slot: " + slotToSwitchTo);
            _currentlyUsedWeaponSlotIndex = slotToSwitchTo;
        }
    }

    void WeaponSlotChange(int currentSlot, int slotToSwitchTo)
    {
        if (slotToSwitchTo >= _weaponSlots.Count) return;
        if (_weaponSlots[slotToSwitchTo] == null) return;
        _currentlyUsedWeaponSlotIndex = slotToSwitchTo;
        _currentlyUsedWeaponSlot = _weaponSlots[_currentlyUsedWeaponSlotIndex];
        
        PlayerInventoryEvents.RaiseWeaponSwitchEvent(_currentlyUsedWeaponSlot.Weapon, _currentlyUsedWeaponSlot.Script);
    }

    void SetDefaultWeapon()
    {
        if (_inventoryData.WeaponInventory.WeaponGOs.Count == 0)
        {
            Debug.Log("Inventory has no weapons...");
            return;
        }

        _currentlyUsedWeaponSlotIndex = 0;
        _currentlyUsedWeaponSlot = _weaponSlots[_currentlyUsedWeaponSlotIndex];

        if (_currentlyUsedWeaponSlot != null)
        {
            _currentlyUsedWeaponSlot.Weapon.SetActive(true);
            PlayerInventoryEvents.RaiseInventoryReadyEvent(_currentlyUsedWeaponSlot.Weapon, _currentlyUsedWeaponSlot.Script);
        }
        
    }

    void SetDefaultEquipment()
    {
        _currentEquipment = _inventoryData.EquipmentInventory.EquipmentGOs.ElementAt(_currentlyUsedEquipmentSlot);
        _currentEquipmentScript = _inventoryData.EquipmentInventory.EquipmentScripts.ElementAt(_currentlyUsedEquipmentSlot);

        if (_currentEquipment && _currentEquipmentScript)
        {
            _currentEquipment.SetActive(false);
            PlayerInventoryEvents.RaiseEquipmentReadyEvent(_currentEquipment, _currentEquipmentScript);
        }
    }

    public bool CanPickupWeapon(WeaponConfigSO weaponToPickUp)
    {
        if (_inventoryData.WeaponInventory.CanPickupWeapon(weaponToPickUp) != true)
            return false;

        return true;
    }

    public void TryAddWeapon(WeaponConfigSO config)
    {

        if (config == null)
        {
            Debug.LogError("Config is null. Inventory can't add weapon.");
            return;
        }

        bool wasWeaponInventoryEmpty = _inventoryData.WeaponInventory.IsEmpty();

        GameObject weaponGO = Instantiate(config.Prefab);
        BaseWeapon weaponScript = weaponGO.GetComponent<BaseWeapon>();
        SetupWeapon(weaponGO, weaponScript);
        weaponScript.InitializeWithConfig();

        

        _inventoryData.WeaponInventory.AddWeaponToInventory(weaponGO, weaponScript);
        WeaponSlot slot = new(weaponGO, weaponScript);
        _weaponSlots.Add(slot);

        if (wasWeaponInventoryEmpty)
        {
            SwitchToWeaponSlot(0);
        }
    }

    public void TryAddWeapon(WeaponRuntimeData runtimeData)
    {
        
        if (runtimeData == null || runtimeData.Config == null)
        {
            Debug.LogError("Config and/or runtime data are/is null.");
            return;
        }

        bool wasWeaponInventoryEmpty = _inventoryData.WeaponInventory.IsEmpty();

        GameObject weaponGO = Instantiate(runtimeData.Config.Prefab);
        BaseWeapon weaponScript = weaponGO.GetComponent<BaseWeapon>();
        SetupWeapon(weaponGO, weaponScript);
        weaponScript.InitializeWithRuntimeData(runtimeData);

        _inventoryData.WeaponInventory.AddWeaponToInventory(weaponGO, weaponScript);
        WeaponSlot slot = new(weaponGO, weaponScript);
        _weaponSlots.Add(slot);

        if (wasWeaponInventoryEmpty)
        {
            SwitchToWeaponSlot(0);
        }

    }

    void SetupWeapon(GameObject weaponGO, BaseWeapon weaponScript)
    {
        weaponGO.transform.parent = _weaponParent;
        weaponGO.transform.SetPositionAndRotation(_weaponPosition.position, _weaponPosition.rotation);
        weaponGO.transform.localPosition += weaponScript.WeaponConfig.offset;
        weaponGO.SetActive(false);
    }

    void DropCurrentWeapon()
    {
        if (_currentlyUsedWeaponSlot == null)
        {
            Debug.Log("Can't drop NULL weapon.");
            return;
        }

        if (_currentlyUsedWeaponSlot.Script.CanBeDropped() == false) return;

        _currentlyUsedWeaponSlot.Script.Drop();
        _inventoryData.WeaponInventory.RemoveWeaponFromInventory(_currentlyUsedWeaponSlot.Weapon, _currentlyUsedWeaponSlot.Script);
        _currentlyUsedWeaponSlot.ClearSlot();
        _weaponSlots.RemoveAt(_currentlyUsedWeaponSlotIndex);

        // if after dropping the weapon, we don't have any weapons in the inventory left
        if (_weaponSlots.Count == 0)
        {
            Debug.Log("No more weapons left doe...");
            _currentlyUsedWeaponSlot = null;
            _currentlyUsedWeaponSlotIndex = -1;
            PlayerInventoryEvents.RaiseWeaponSwitchEvent(null, null);
            return;
        }

        // we drop the last weapon on our weapon slots but still have weapons to fallback on
        else if (_currentlyUsedWeaponSlotIndex >= _weaponSlots.Count)
        {
            int slotToSwitchTo = _currentlyUsedWeaponSlotIndex - 1;
            SwitchToWeaponSlot(slotToSwitchTo);
        }

        // the dropped weapon isn't on the last slot, we can stay on the same slot as
        // slots shift to the left
        else
        {
            int slotToSwitchTo = _currentlyUsedWeaponSlotIndex;
            _currentlyUsedWeaponSlotIndex = -1;
            SwitchToWeaponSlot(slotToSwitchTo);

        }
            
    }


    /*
    void OutOfAmmoAutoSwitch()
    {
        if (_isHandlingEmptyWeapon) return;

        Debug.Log("Handling empty weapon now.");
        _isHandlingEmptyWeapon = true;
        StartCoroutine(AutoSwitchAfterTime());
    }

    IEnumerator AutoSwitchAfterTime()
    {
        Debug.Log("Trying to auto switch to next weapon...");
        float gracePeriod = 0.5f;
        yield return new WaitForSeconds(gracePeriod);

        int validWeaponSlot = _inventoryData.WeaponInventory.FindNextWeaponWithAmmo(_currentlyUsedWeaponSlot);
        if (validWeaponSlot != -1)
        {
            SwitchToWeaponSlot(validWeaponSlot);
        }

        _isHandlingEmptyWeapon = false;
    }
    */    

}
