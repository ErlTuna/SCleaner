using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] WeaponUIUpdateEventChannelSO _weaponUIUpdateEventChannel;
    [SerializeField] WeaponSwitchRequestEventChannelSO _weaponSwitchRequestEventChannel;
    [SerializeField] UnitInventoryData _playerInventoryData;
    [SerializeField] UnitInventoryConfigSO _playerInventoryConfig;
    [SerializeField] Transform _weaponParent;
    [SerializeField] Transform _weaponPosition;
    [SerializeField] Transform _equipmentPosition;
    [SerializeReference] List<WeaponSlot> _weaponSlots = new();
    [SerializeField] EquipmentSlot _currentlyUsedEquipmentSlot;
    WeaponSlot _currentlyUsedWeaponSlot;
    int _currentlyUsedWeaponSlotIndex = -1;

    IEnumerator Start()
    {
        yield return new WaitUntil(() => _playerInventoryConfig != null);
        PrepareWeapons();
        //SetDefaultWeapon();
        PrepareEquipment();
        SetDefaultEquipment();
    }

    public void InitializeManager(UnitInventoryData unitInventoryData, UnitInventoryConfigSO unitInventoryConfig)
    {
        _playerInventoryData = unitInventoryData;
        _playerInventoryConfig = unitInventoryConfig;
    }

    void Update()
    {
        if (PlayerInputManager.Instance.WeaponDropInput)
            DropCurrentWeapon();

        if (PlayerInputManager.Instance.WeaponSwitchInput != -1)
            SwitchToWeaponSlot(PlayerInputManager.Instance.WeaponSwitchInput);
    }

    void PrepareWeapons()
    {
        int currentSlot = 0;
        foreach (GameObject weaponPrefab in _playerInventoryConfig.WeaponPrefabs)
        {
            GameObject weaponGO = Instantiate(weaponPrefab);
            weaponGO.transform.parent = _weaponParent;
            weaponGO.transform.SetPositionAndRotation(_weaponPosition.position, _weaponPosition.rotation);

            PlayerWeapon weaponScript = weaponGO.GetComponent<PlayerWeapon>();
            weaponScript.InitializeWithConfig();

            weaponGO.transform.localPosition += weaponScript.WeaponConfig.offset;

            if (weaponScript != null)
            {
                _playerInventoryData.WeaponInventory.AddWeaponToInventory(weaponGO, weaponScript);
                WeaponSlot slot = new(weaponGO, weaponScript);
                _weaponSlots.Add(slot);
                weaponGO.SetActive(false);
            }

            currentSlot++;
        }
    }

    void PrepareEquipment()
    {
            GameObject equipmentGO = Instantiate(_playerInventoryConfig.EquipmentPrefab);
            equipmentGO.transform.parent = _weaponParent;
            equipmentGO.transform.position = _equipmentPosition.position;

            BaseEquipment equipmentScript = equipmentGO.GetComponent<BaseEquipment>();
            equipmentScript.InitializeEquipment();
            if (equipmentScript != null)
            {
                _playerInventoryData.EquipmentInventory.AddEquipment(equipmentGO, equipmentScript);
                _currentlyUsedEquipmentSlot.Equipment = equipmentGO;
                _currentlyUsedEquipmentSlot.Script = equipmentScript;
                equipmentGO.SetActive(false);
            }

            PlayerInventoryEvents.RaiseEquipmentReadyEvent(equipmentGO, equipmentScript);
            PlayerInventoryEvents.RaiseEquipmentSwitchUIUpdateEvent(equipmentScript.EquipmentConfig.UI_Icon, equipmentScript.EquipmentData);
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
        
        //PlayerInventoryEvents.RaiseWeaponSwitchEvent(_currentlyUsedWeaponSlot.Weapon, _currentlyUsedWeaponSlot.Script);

        

        if (_weaponUIUpdateEventChannel != null)
        {
            WeaponUpdateData weaponUpdateData = new(
            _currentlyUsedWeaponSlot.Script.WeaponConfig.UI_Icon,
            _currentlyUsedWeaponSlot.Script.WeaponRuntimeData.CurrentAmmo,
             _currentlyUsedWeaponSlot.Script.WeaponRuntimeData.ReserveAmmo
            );
            _weaponUIUpdateEventChannel.RaiseEvent(weaponUpdateData);
        }

        if (_weaponSwitchRequestEventChannel != null)
            _weaponSwitchRequestEventChannel.RaiseEvent(_currentlyUsedWeaponSlot.Weapon, _currentlyUsedWeaponSlot.Script);
    }

    void SetDefaultWeapon()
    {
        if (_playerInventoryData.WeaponInventory.WeaponGOs.Count == 0)
        {
            Debug.Log("Inventory has no weapons...");
            return;
        }

        _currentlyUsedWeaponSlotIndex = 0;
        _currentlyUsedWeaponSlot = _weaponSlots[_currentlyUsedWeaponSlotIndex];

        if (_currentlyUsedWeaponSlot != null)
        {
            _currentlyUsedWeaponSlot.Weapon.SetActive(true);
            PlayerInventoryEvents.RaiseWeaponsReadyEvent(_currentlyUsedWeaponSlot.Weapon, _currentlyUsedWeaponSlot.Script);
        }
        
    }

    void SetDefaultEquipment()
    {
        /*
        _currentEquipment = _playerInventoryData.EquipmentInventory.EquipmentGOs.ElementAt(_currentlyUsedEquipmentSlot);
        _currentEquipmentScript = _playerInventoryData.EquipmentInventory.EquipmentScripts.ElementAt(_currentlyUsedEquipmentSlot);

        if (_currentEquipment && _currentEquipmentScript)
        {
            _currentEquipment.SetActive(false);
            PlayerInventoryEvents.RaiseEquipmentReadyEvent(_currentEquipment, _currentEquipmentScript);
        }
        */
    }

    public bool CanPickupWeapon(WeaponConfigSO weaponToPickUp)
    {
        if (_playerInventoryData.WeaponInventory.CanPickupWeapon(weaponToPickUp) != true)
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

        bool wasWeaponInventoryEmpty = _playerInventoryData.WeaponInventory.IsEmpty();

        GameObject weaponGO = Instantiate(config.Prefab);
        PlayerWeapon weaponScript = weaponGO.GetComponent<PlayerWeapon>();
        SetupWeapon(weaponGO, weaponScript);
        weaponScript.InitializeWithConfig();

        

        _playerInventoryData.WeaponInventory.AddWeaponToInventory(weaponGO, weaponScript);
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

        bool wasWeaponInventoryEmpty = _playerInventoryData.WeaponInventory.IsEmpty();

        GameObject weaponGO = Instantiate(runtimeData.Config.Prefab);
        PlayerWeapon weaponScript = weaponGO.GetComponent<PlayerWeapon>();
        SetupWeapon(weaponGO, weaponScript);
        weaponScript.InitializeWithRuntimeData(runtimeData);

        _playerInventoryData.WeaponInventory.AddWeaponToInventory(weaponGO, weaponScript);
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
        weaponGO.transform.SetLocalPositionAndRotation(weaponScript.WeaponConfig.offset, Quaternion.identity);
        //weaponGO.transform.SetPositionAndRotation(_weaponPosition.position, _weaponPosition.rotation);
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
        _playerInventoryData.WeaponInventory.RemoveWeaponFromInventory(_currentlyUsedWeaponSlot.Weapon, _currentlyUsedWeaponSlot.Script);
        _currentlyUsedWeaponSlot.ClearSlot();
        _weaponSlots.RemoveAt(_currentlyUsedWeaponSlotIndex);

        // if after dropping the weapon, we don't have any weapons in the inventory left
        if (_weaponSlots.Count == 0)
        {
            _currentlyUsedWeaponSlot = null;
            _currentlyUsedWeaponSlotIndex = -1;
            WeaponUpdateData weaponUpdateData = new();

            //PlayerInventoryEvents.RaiseWeaponSwitchEvent(null, null);
            _weaponUIUpdateEventChannel.RaiseEvent(weaponUpdateData);
            return;
        }

        // we drop the last weapon on our weapon slots but still have weapons to fallback on
        else if (_currentlyUsedWeaponSlotIndex >= _weaponSlots.Count)
        {
            int slotToSwitchTo = _currentlyUsedWeaponSlotIndex - 1;
            SwitchToWeaponSlot(slotToSwitchTo);
        }

        // the dropped weapon isn't on the last slot, we can stay on the same slot
        // as slots shift to the left
        else
        {
            int slotToSwitchTo = _currentlyUsedWeaponSlotIndex;
            _currentlyUsedWeaponSlotIndex = -1;
            SwitchToWeaponSlot(slotToSwitchTo);

        }
            
    }
}
