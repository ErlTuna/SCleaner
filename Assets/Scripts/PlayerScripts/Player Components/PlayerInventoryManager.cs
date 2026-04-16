using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour, IWeaponPickupHandler, IPassiveItemPickupHandler, ICurrencyPickupHandler
{
    [Header("Event Channels")]
    [SerializeField] WeaponUIUpdateEventChannelSO _weaponUIUpdateEventChannel;
    [SerializeField] WeaponSwitchRequestEventChannelSO _weaponSwitchRequestEventChannel;

    [Header("Config and Runtime Data")]
    [SerializeField] UnitInventoryData _playerInventoryData;
    [SerializeField] PlayerInventoryConfigSO _playerInventoryConfig;

    PlayerInventoryRuntime _inventoryRuntime;
    

    [Header("Transforms")]
    [SerializeField] Transform _weaponParent;
    [SerializeField] Transform _weaponPosition;
    [SerializeField] Transform _equipmentPosition;

    [Header("Slots")]
    [SerializeField] List<WeaponSlot> _weaponSlots = new();
    [SerializeField] EquipmentSlot _currentlyUsedEquipmentSlot;
    WeaponSlot _currentlyUsedWeaponSlot;
    int _currentlyUsedWeaponSlotIndex = -1;


    // Some flags?

    bool _isInitialized = false;

    public void InitializeManager(PlayerInventoryRuntime inventoryRuntime, PlayerInventoryConfigSO playerInventoryConfig)
    {
        _inventoryRuntime = inventoryRuntime;
        _playerInventoryConfig = playerInventoryConfig;
        if (_isInitialized == false)
            InitializeWeaponSlots();
    }

    IEnumerator Start()
    {
        yield return null;
        SwitchToWeaponSlot(0);
    }

    void Update()
    {
        if (PlayerInputManager.Instance.WeaponDropInput)
            DropCurrentWeapon();

        if (PlayerInputManager.Instance.WeaponSwitchInput != -1)
            SwitchToWeaponSlot(PlayerInputManager.Instance.WeaponSwitchInput);
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
            WeaponSlotChange(slotToSwitchTo);
            _currentlyUsedWeaponSlotIndex = slotToSwitchTo;
        }
    }

    void WeaponSlotChange(int slotToSwitchTo)
    {
        if (slotToSwitchTo >= _weaponSlots.Count)
         return;
        if (_weaponSlots[slotToSwitchTo] == null) return;


        _currentlyUsedWeaponSlotIndex = slotToSwitchTo;
        _currentlyUsedWeaponSlot = _weaponSlots[_currentlyUsedWeaponSlotIndex];
                

        if (_weaponUIUpdateEventChannel != null)
        {
            
            UIWeaponSwitchContext weaponUpdateData = new
            (
                _currentlyUsedWeaponSlot.WeaponInstance.WeaponScript.WeaponConfig.InventoryItemDefinition.ItemIcon,
                _currentlyUsedWeaponSlot.WeaponInstance.WeaponScript.WeaponRuntimeData.AmmoData.CurrentAmmo,
                _currentlyUsedWeaponSlot.WeaponInstance.WeaponScript.WeaponRuntimeData.AmmoData.CurrentReserveAmmo,
                _currentlyUsedWeaponSlot.WeaponInstance.WeaponScript.WeaponConfig.AmmoConfig.HasInfiniteReserveAmmo
            );
            

            _weaponUIUpdateEventChannel.RaiseEvent(weaponUpdateData);
        }

        Debug.Log("RAISING SWITCH EVENT: " + _weaponSwitchRequestEventChannel.GetInstanceID());

        if (_weaponSwitchRequestEventChannel != null)
            _weaponSwitchRequestEventChannel.RaiseEvent(_currentlyUsedWeaponSlot.WeaponInstance.WeaponGO, _currentlyUsedWeaponSlot.WeaponInstance.WeaponScript);

        Debug.Log("Switched to weapon slot: " + slotToSwitchTo);
    }

    // Configs are unique per weapon. We can use that to check if a weapon exists.
    /*
    public bool CanPickupWeapon(PlayerWeaponConfigSO playerWeaponConfig)
    {
        return _playerInventoryData.WeaponInventory.CanPickupWeapon(playerWeaponConfig);
    }
    */
    public bool CanPickupWeapon(PlayerWeaponConfigSO playerWeaponConfig)
    {
        return _inventoryRuntime.WeaponInventoryRuntime.CanPickupWeapon(playerWeaponConfig);
    }

    /*
    public void AddPickedUpWeapon(WeaponPickupPayload payload)
    {   

        bool wasEmpty = _playerInventoryData.WeaponInventory.IsEmpty();

        (PlayerWeapon weaponScript, GameObject weaponGO) = WeaponFactory.CreateUsingRuntimeData(payload.RuntimeData);

        _playerInventoryData.WeaponInventory.AddWeaponToInventory(weaponGO, weaponScript);
        SetupWeapon(weaponGO, weaponScript);
        _weaponSlots.Add(new WeaponSlot(weaponGO, weaponScript));

        if (wasEmpty)
            SwitchToWeaponSlot(0);
    }
    */

    public void AddPickedUpWeapon(WeaponPickupPayload payload)
    {   
        bool wasEmpty = _inventoryRuntime.WeaponInventoryRuntime.IsEmpty();

        (PlayerWeapon weaponScript, GameObject weaponGO) = WeaponFactory.CreateUsingRuntimeData(payload.RuntimeData);

        WeaponInstance createdWeaponInstance = _inventoryRuntime.WeaponInventoryRuntime.AddWeaponToInventory(weaponGO, weaponScript);
        AttachWeaponToPlayer(weaponGO, weaponScript);
        _weaponSlots.Add(new WeaponSlot(createdWeaponInstance));

        if (wasEmpty)
            SwitchToWeaponSlot(0);
    }


    void AttachWeaponToPlayer(GameObject weaponGO, PlayerWeapon weaponScript)
    {
        weaponGO.transform.parent = _weaponParent;
        weaponGO.transform.SetLocalPositionAndRotation(weaponScript.WeaponConfig.offset, Quaternion.identity);
        weaponScript.AttachOwnerTransform(transform);
        weaponGO.SetActive(false);
    }

    void DropCurrentWeapon()
    {
        if (_currentlyUsedWeaponSlot == null)
        {
            Debug.Log("Can't drop NULL weapon.");
            return;
        }

        if (_currentlyUsedWeaponSlot.WeaponInstance.WeaponScript.CanBeDropped() == false) return;

        _currentlyUsedWeaponSlot.WeaponInstance.WeaponScript.Drop();
        //_playerInventoryData.WeaponInventory.RemoveWeaponFromInventory(_currentlyUsedWeaponSlot.Weapon, _currentlyUsedWeaponSlot.Script);

        _inventoryRuntime.WeaponInventoryRuntime.RemoveWeaponFromInventory(_currentlyUsedWeaponSlot.WeaponInstance.WeaponID);
        _currentlyUsedWeaponSlot.ClearSlot();
        _weaponSlots.RemoveAt(_currentlyUsedWeaponSlotIndex);

        // if after dropping the weapon, we don't have any weapons in the inventory left
        if (_weaponSlots.Count == 0)
        {
            _currentlyUsedWeaponSlot = null;
            _currentlyUsedWeaponSlotIndex = -1;
            UIWeaponSwitchContext weaponUpdateData = new();

            _weaponUIUpdateEventChannel.RaiseEvent(weaponUpdateData);

            _weaponSwitchRequestEventChannel.RaiseEvent(null, null);
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

    public bool CanAddPassiveItem(PassiveItemSO passiveItem)
    {
        return _inventoryRuntime.PassiveItemInventoryRuntime.HasItem(passiveItem) == false;
    }

    public void AddPassiveItem(PassiveItemPickupPayload payload)
    {
        _inventoryRuntime.PassiveItemInventoryRuntime.AddPassiveItemToInventory(payload.PassiveItemSO);
    }

    public void AddCurrency(int amont)
    {
        _inventoryRuntime.CurrencyInventoryRuntime.AddCurrency(amont);
    }


    // CHANGE THIS TO REBUILDING, NOT CREATING!
    void PrepareDefaultWeapon()
    {
        if (_playerInventoryConfig.DefaultWeaponConfig == null) return;

        (PlayerWeapon weaponScript, GameObject weaponGO) = WeaponFactory.CreateUsingConfig(_playerInventoryConfig.DefaultWeaponConfig);

        WeaponInstance createdWeaponInstance = _inventoryRuntime.WeaponInventoryRuntime.AddWeaponToInventory(weaponGO, weaponScript);
        AttachWeaponToPlayer(weaponGO, weaponScript);
        _weaponSlots.Add(new WeaponSlot(createdWeaponInstance));
        

        SwitchToWeaponSlot(0);

        _isInitialized = true;
    }

    void InitializeWeaponSlots()
    {
        _weaponSlots.Clear();

        (PlayerWeapon weaponScript, GameObject weaponGO) = WeaponFactory.CreateUsingConfig(_playerInventoryConfig.DefaultWeaponConfig);
        _inventoryRuntime.WeaponInventoryRuntime.AddWeaponToInventory(weaponGO, weaponScript);

        foreach (WeaponInstance weapon in _inventoryRuntime.WeaponInventoryRuntime.Weapons)
        {
            _weaponSlots.Add(new WeaponSlot(weapon));
            AttachWeaponToPlayer(weapon.WeaponGO, weapon.WeaponScript);
        }

        //SwitchToWeaponSlot(0);
        _isInitialized = true;
    }

}

    /*
    void PrepareWeapons()
    {
        int currentSlot = 0;
        foreach (GameObject weaponPrefab in _playerInventoryConfig.WeaponPrefabs)
        {
            GameObject weaponGO = Instantiate(weaponPrefab);

            PlayerWeapon weaponScript = weaponGO.GetComponent<PlayerWeapon>();
            SetupWeapon(weaponGO, weaponScript);

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
    */

    /*

    void SetupWeapon(GameObject weaponGO, PlayerWeapon weaponScript)
    {
        weaponGO.transform.parent = _weaponParent;
        weaponGO.transform.SetLocalPositionAndRotation(weaponScript.WeaponConfig.offset, Quaternion.identity);
        weaponScript.AttachOwnerTransform(transform);
        weaponGO.SetActive(false);
    }

*/


    /*
    IEnumerator Start()
    {
        yield return new WaitUntil(() => _playerInventoryConfig != null);
        //PrepareWeapons();
        //SetDefaultWeapon();
        //PrepareEquipment();
        //SetDefaultEquipment();
    }
    */

    /*
        void SetDefaultEquipment()
    {
        
        _currentEquipment = _playerInventoryData.EquipmentInventory.EquipmentGOs.ElementAt(_currentlyUsedEquipmentSlot);
        _currentEquipmentScript = _playerInventoryData.EquipmentInventory.EquipmentScripts.ElementAt(_currentlyUsedEquipmentSlot);

        if (_currentEquipment && _currentEquipmentScript)
        {
            _currentEquipment.SetActive(false);
            PlayerInventoryEvents.RaiseEquipmentReadyEvent(_currentEquipment, _currentEquipmentScript);
        }
        
    }
    */

    /*
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

    }
    */





    