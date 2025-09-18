using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventoryManager_v2 : MonoBehaviour
{
    Unit _owner;
    PlayerInventoryData _inventoryData;
    [SerializeField] Transform _weaponParent;
    [SerializeField] Transform _weaponPosition;
    public GameObject currentWeapon;
    public BaseWeapon_v2 currentWeaponScript;
    public int weaponCount;
    public int currentlyUsedSlot = 0;

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

    public void InitializeWithData(Unit owner, PlayerInventoryData inventoryData)
    {
        _owner = owner;
        _inventoryData = inventoryData;


        PrepareWeapons();
        SetDefaultWeapon();
        
    }

    void PrepareWeapons()
    {
        foreach (GameObject weaponPrefab in _inventoryData.weaponPrefabs)
        {
            GameObject weaponGO = Instantiate(weaponPrefab);
            weaponGO.transform.parent = _weaponParent;
            weaponGO.transform.position = _weaponPosition.position;
            
            BaseWeapon_v2 weaponScript = weaponGO.GetComponent<BaseWeapon_v2>();
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

    void SwitchWeapons(int slot)
    {
        int slotToSwitchTo;

        switch (slot)
        {
            case 0:
                slotToSwitchTo = 0;
                if (_inventoryData.WeaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                {
                    WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                }

                break;
            case 1:
                slotToSwitchTo = 1;
                if (_inventoryData.WeaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                {
                    WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                }

                break;
            case 2:
                slotToSwitchTo = 2;
                if (_inventoryData.WeaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                {
                    WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                }
                break;
            case 3:
                slotToSwitchTo = 3;
                if (_inventoryData.WeaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                {
                    WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                }
                break;
            default:
                break;
        }
    }

    void WeaponSlotChange(int currentSlot, int slotToSwitchTo)
    {
        GameObject targetWeapon = _inventoryData.WeaponGOs[slotToSwitchTo];
        BaseWeapon_v2 targetWeaponScript = _inventoryData.WeaponScripts[slotToSwitchTo];

        currentWeapon = targetWeapon;
        currentWeaponScript = targetWeaponScript;
        currentlyUsedSlot = slotToSwitchTo;


        PlayerInventoryEvents.RaiseWeaponSwitchEvent(currentWeapon, currentWeaponScript);
    }

    void SetDefaultWeapon()
    {
        currentWeapon = _inventoryData.WeaponGOs.ElementAt(currentlyUsedSlot);
        currentWeaponScript = _inventoryData.WeaponScripts.ElementAt(currentlyUsedSlot);
        currentWeapon.SetActive(true);

        PlayerInventoryEvents.RaiseInventoryReadyEvent(currentWeapon, currentWeaponScript);
    }
}
