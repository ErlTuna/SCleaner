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


    [SerializeField] UnitInventoryData_V2 _playerInventoryData_V2;
    PlayerWeaponInventoryRuntime _weaponInventoryRuntime;

    [Header("Transforms")]
    [SerializeField] Transform _weaponParent;
    [SerializeField] Transform _weaponPosition;
    [SerializeField] Transform _equipmentPosition;

    [Header("Slots")]
    [SerializeReference] List<WeaponSlot> _weaponSlots = new();
    [SerializeField] EquipmentSlot _currentlyUsedEquipmentSlot;
    WeaponSlot _currentlyUsedWeaponSlot;
    int _currentlyUsedWeaponSlotIndex = -1;


    // Some flags?

    bool _isInitialized = false;

    void Start()
    {
        if (_isInitialized == false)
            InitializeWeaponSlots();
            //PrepareDefaultWeapon();
    }

    public void InitializeManager(UnitInventoryData unitInventoryData, PlayerInventoryConfigSO unitInventoryConfig)
    {
        _playerInventoryData = unitInventoryData;
        _playerInventoryConfig = unitInventoryConfig;
    }

    public void InitializeManager_V2(PlayerWeaponInventoryRuntime weaponInventoryRuntime, PlayerInventoryConfigSO playerInventoryConfig)
    {
        _weaponInventoryRuntime = weaponInventoryRuntime;
        _playerInventoryConfig = playerInventoryConfig;
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
            Debug.Log("Switched to weapon slot: " + slotToSwitchTo);
            _currentlyUsedWeaponSlotIndex = slotToSwitchTo;
        }
    }

    void WeaponSlotChange(int slotToSwitchTo)
    {
        if (slotToSwitchTo >= _weaponSlots.Count) return;
        if (_weaponSlots[slotToSwitchTo] == null) return;

        _currentlyUsedWeaponSlotIndex = slotToSwitchTo;
        _currentlyUsedWeaponSlot = _weaponSlots[_currentlyUsedWeaponSlotIndex];
                

        if (_weaponUIUpdateEventChannel != null)
        {
            
            UIWeaponSwitchContext weaponUpdateData = new
            (
                _currentlyUsedWeaponSlot.Script.WeaponConfig.InventoryItemDefinition.ItemIcon,
                _currentlyUsedWeaponSlot.Script.WeaponRuntimeData.AmmoData.CurrentAmmo,
                _currentlyUsedWeaponSlot.Script.WeaponRuntimeData.AmmoData.CurrentReserveAmmo,
                _currentlyUsedWeaponSlot.Script.WeaponConfig.AmmoConfig.HasInfiniteReserveAmmo
            );
            

            _weaponUIUpdateEventChannel.RaiseEvent(weaponUpdateData);
        }

        if (_weaponSwitchRequestEventChannel != null)
            _weaponSwitchRequestEventChannel.RaiseEvent(_currentlyUsedWeaponSlot.Weapon, _currentlyUsedWeaponSlot.Script);
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
        return _weaponInventoryRuntime.CanPickupWeapon(playerWeaponConfig);
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
        bool wasEmpty = _weaponInventoryRuntime.IsEmpty();

        (PlayerWeapon weaponScript, GameObject weaponGO) = WeaponFactory.CreateUsingRuntimeData(payload.RuntimeData);

        _weaponInventoryRuntime.AddWeaponToInventory(weaponGO, weaponScript);
        AttachWeaponToPlayer(weaponGO, weaponScript);
        _weaponSlots.Add(new WeaponSlot(weaponGO, weaponScript));

        if (wasEmpty)
            SwitchToWeaponSlot(0);
    }

    public void AddPickedUpWeapon_v2(WeaponPickupPayload payload)
    {   
        bool wasEmpty = _weaponInventoryRuntime.IsEmpty();

        (PlayerWeapon weaponScript, GameObject weaponGO) = WeaponFactory.CreateUsingRuntimeData(payload.RuntimeData);

        //WeaponInstance weaponInstance = _weaponInventoryRuntime.AddWeaponToInventory();

        _weaponInventoryRuntime.AddWeaponToInventory(weaponGO, weaponScript);
        AttachWeaponToPlayer(weaponGO, weaponScript);
        _weaponSlots.Add(new WeaponSlot(weaponGO, weaponScript));

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

        if (_currentlyUsedWeaponSlot.Script.CanBeDropped() == false) return;

        _currentlyUsedWeaponSlot.Script.Drop();
        //_playerInventoryData.WeaponInventory.RemoveWeaponFromInventory(_currentlyUsedWeaponSlot.Weapon, _currentlyUsedWeaponSlot.Script);

        _weaponInventoryRuntime.RemoveWeaponFromInventory(_currentlyUsedWeaponSlot.Weapon, _currentlyUsedWeaponSlot.Script);
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
        return _playerInventoryData.PassiveItemInventory.HasItem(passiveItem) == false;
    }

    public void AddPassiveItem(PassiveItemPickupPayload payload)
    {
        _playerInventoryData.PassiveItemInventory.AddPassiveItemToInventory(payload.PassiveItemSO);
    }

    public void AddCurrency(int amont)
    {
        _playerInventoryData.CurrencyInventory.AddCurrency(amont);
    }


    // CHANGE THIS TO REBUILDING, NOT CREATING!
    void PrepareDefaultWeapon()
    {
        if (_playerInventoryConfig.DefaultWeaponConfig == null) return;

        (PlayerWeapon weaponScript, GameObject weaponGO) = WeaponFactory.CreateUsingConfig(_playerInventoryConfig.DefaultWeaponConfig);

        _playerInventoryData.WeaponInventory.AddWeaponToInventory(weaponGO, weaponScript);
        AttachWeaponToPlayer(weaponGO, weaponScript);
        _weaponSlots.Add(new WeaponSlot(weaponGO, weaponScript));
        

        SwitchToWeaponSlot(0);

        _isInitialized = true;
    }

    void InitializeWeaponSlots()
    {
        _weaponSlots.Clear();

        (PlayerWeapon weaponScript, GameObject weaponGO) = WeaponFactory.CreateUsingConfig(_playerInventoryConfig.DefaultWeaponConfig);
        _weaponInventoryRuntime.AddWeaponToInventory(weaponGO, weaponScript);
        


        _weaponInventoryRuntime.RebuildFromData(_weaponParent);

        foreach (WeaponInstance weapon in _weaponInventoryRuntime.Weapons)
        {
            _weaponSlots.Add(new WeaponSlot(weapon.WeaponGO, weapon.WeaponScript));
            AttachWeaponToPlayer(weapon.WeaponGO, weapon.WeaponScript);
        }

        SwitchToWeaponSlot(0);
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





    