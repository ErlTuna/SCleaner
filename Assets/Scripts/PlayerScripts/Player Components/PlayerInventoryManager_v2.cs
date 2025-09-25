using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventoryManager_v2 : MonoBehaviour
{
    Unit _owner;
    UnitInventoryData _inventoryData;
    [SerializeField] Transform _weaponParent;
    [SerializeField] Transform _weaponPosition;
    [SerializeField] Transform _equipmentPosition;
    public GameObject currentWeapon;
    public BaseWeapon currentWeaponScript;
    public GameObject currentEquipment;
    public BaseEquipment currentEquipmentScript;
    public int weaponCount;
    public int currentlyUsedWeaponSlot = 0;

    void Update()
    {
        if (PlayerInputManager.instance.FirstWeaponInput)
            SwitchWeapons(0);
        else if (PlayerInputManager.instance.SecondWeaponInput)
            SwitchWeapons(1);
        else if (PlayerInputManager.instance.ThirdWeaponInput)
            SwitchWeapons(2);
        else if (PlayerInputManager.instance.FourthWeaponInput)
            SwitchWeapons(3);
    }

    public void InitializeWithData(Unit owner, UnitInventoryData inventoryData)
    {
        _owner = owner;
        _inventoryData = inventoryData;


        PrepareWeapons();
        SetDefaultWeapon();
        PrepareEquipments();
        SetDefaultEquipment();
    }

    void PrepareWeapons()
    {
        foreach (GameObject weaponPrefab in _inventoryData.WeaponPrefabs)
        {
            GameObject weaponGO = Instantiate(weaponPrefab);
            weaponGO.transform.parent = _weaponParent;
            weaponGO.transform.position = _weaponPosition.position;

            BaseWeapon weaponScript = weaponGO.GetComponent<BaseWeapon>();
            weaponScript.InitializeWithData();

            weaponGO.transform.localPosition += weaponScript.WeaponConfig.offset;

            if (weaponScript != null)
            {
                _inventoryData.WeaponGOs.Add(weaponGO);
                _inventoryData.WeaponScripts.Add(weaponScript);
                weaponGO.SetActive(false);
            }
        }
    }

    void PrepareEquipments()
    {
        foreach (GameObject equipmentPrefab in _inventoryData.EquipmentPrefabs)
        {
            GameObject equipmentGO = Instantiate(equipmentPrefab);
            equipmentGO.transform.parent = _weaponParent;
            equipmentGO.transform.position = _equipmentPosition.position;

            BaseEquipment equipmentScript = equipmentGO.GetComponent<BaseEquipment>();
            equipmentScript.InitializeWithData();

            //equipmentGO.transform.localPosition += equip.WeaponConfig.offset;

            if (equipmentScript != null)
            {
                _inventoryData.EquipmentGOs.Add(equipmentGO);
                _inventoryData.EquipmentScripts.Add(equipmentScript);
                equipmentGO.SetActive(false);
            }
        }
    }

    void SwitchWeapons(int slot)
    {
        int slotToSwitchTo;

        switch (slot)
        {
            case 0:
                slotToSwitchTo = 0;
                if (_inventoryData.WeaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedWeaponSlot)
                {
                    WeaponSlotChange(currentlyUsedWeaponSlot, slotToSwitchTo);
                }

                break;
            case 1:
                slotToSwitchTo = 1;
                if (_inventoryData.WeaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedWeaponSlot)
                {
                    WeaponSlotChange(currentlyUsedWeaponSlot, slotToSwitchTo);
                }

                break;
            case 2:
                slotToSwitchTo = 2;
                if (_inventoryData.WeaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedWeaponSlot)
                {
                    WeaponSlotChange(currentlyUsedWeaponSlot, slotToSwitchTo);
                }
                break;
            case 3:
                slotToSwitchTo = 3;
                if (_inventoryData.WeaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedWeaponSlot)
                {
                    WeaponSlotChange(currentlyUsedWeaponSlot, slotToSwitchTo);
                }
                break;
            default:
                break;
        }
    }

    void WeaponSlotChange(int currentSlot, int slotToSwitchTo)
    {
        GameObject targetWeapon = _inventoryData.WeaponGOs[slotToSwitchTo];
        BaseWeapon targetWeaponScript = _inventoryData.WeaponScripts[slotToSwitchTo];

        currentWeapon = targetWeapon;
        currentWeaponScript = targetWeaponScript;
        currentlyUsedWeaponSlot = slotToSwitchTo;


        PlayerInventoryEvents.RaiseWeaponSwitchEvent(currentWeapon, currentWeaponScript);
    }

    void SetDefaultWeapon()
    {
        currentWeapon = _inventoryData.WeaponGOs.ElementAt(currentlyUsedWeaponSlot);
        currentWeaponScript = _inventoryData.WeaponScripts.ElementAt(currentlyUsedWeaponSlot);
        currentWeapon.SetActive(true);

        PlayerInventoryEvents.RaiseInventoryReadyEvent(currentWeapon, currentWeaponScript);
    }

    void SetDefaultEquipment()
    {
        currentEquipment = _inventoryData.EquipmentGOs.ElementAt(currentlyUsedWeaponSlot);
        currentEquipmentScript = _inventoryData.EquipmentScripts.ElementAt(currentlyUsedWeaponSlot);
        currentEquipment.SetActive(false);

        PlayerInventoryEvents.RaiseEquipmentReadyEvent(currentEquipment, currentEquipmentScript);
        //PlayerInventoryEvents.RaiseInventoryReadyEvent(currentWeapon, currentWeaponScript);
    }
}
