using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class PlayerInventoryManager : MonoBehaviour
{
    #region Events
    public static Action<GameObject, BaseWeapon, WeaponData> OnInventoryReadyEvent;
    public static Action<GameObject, BaseWeapon> OnWeaponSwitchEvent;
    public static Action<WeaponData> OnWeaponSwitchUIUpdate;

    #endregion
    public PlayerInventorySO inventoryData;
    [SerializeField] Transform _weaponParent;
    [SerializeField] Transform _weaponPosition;
    public List<GameObject> weaponGOs;
    public List<BaseWeapon> weaponScripts;
    public GameObject currentWeapon;
    public BaseWeapon currentWeaponScript;
    public int weaponCount;
    public int currentlyUsedSlot = 0;

    void Awake()
    {
        PrepareWeaponGOs();
        weaponCount = weaponGOs.Count();
    }

    void Start()
    {
        PrepareWeaponScripts();
        SetDefaultWeapon();
        OnInventoryReadyEvent?.Invoke(currentWeapon, currentWeaponScript, currentWeaponScript.data);
    }

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

    void SwitchWeapons(int slot)
    {
        if (currentWeaponScript.data.state == WeaponState.FIRING) return;

        int slotToSwitchTo = -1;

        switch (slot)
        {
            case 0:
                slotToSwitchTo = 0;
                if (weaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                {
                    WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                }

                break;
            case 1:
                slotToSwitchTo = 1;
                if (weaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                {
                    WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                }

                break;
            case 2:
                slotToSwitchTo = 2;
                if (weaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                {
                    WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                }
                break;
            case 3:
                slotToSwitchTo = 3;
                if (weaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
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
        OnWeaponSwitchEvent?.Invoke(weaponGOs[slotToSwitchTo], weaponScripts[slotToSwitchTo]);
        OnWeaponSwitchUIUpdate?.Invoke(weaponScripts[slotToSwitchTo].data);
        currentWeapon = weaponGOs[slotToSwitchTo];
        currentWeaponScript = weaponScripts[slotToSwitchTo];
        currentlyUsedSlot = slotToSwitchTo;
    }

    void PrepareWeaponGOs()
    {
        GameObject instantiatedWeaponPrefab;
        for (int i = 0; i < inventoryData.weaponsHeld; ++i)
        {
            if (inventoryData.weaponDatas.ElementAt(i))
            {
                instantiatedWeaponPrefab = Instantiate(inventoryData.weaponDatas[i].prefab);

                weaponGOs.Add(instantiatedWeaponPrefab);
            }
        }
    }

    void PrepareWeaponScripts()
    {
        BaseWeapon instantiatedWeaponScript;
        for (int i = 0; i < weaponCount; ++i)
        {
            if (_weaponParent != null)
            {

                instantiatedWeaponScript = weaponGOs.ElementAt(i).GetComponent<BaseWeapon>();
                if (instantiatedWeaponScript)
                    weaponScripts.Add(instantiatedWeaponScript);

                weaponGOs[i].transform.parent = _weaponParent;
                weaponGOs[i].transform.position = _weaponPosition.position;

                weaponGOs[i].transform.localPosition += instantiatedWeaponScript.data.offset;
                weaponGOs[i].SetActive(false);
            }

        }
    }

    void SetDefaultWeapon()
    {
        currentWeapon = weaponGOs.ElementAt(currentlyUsedSlot);
        currentWeaponScript = weaponScripts.ElementAt(currentlyUsedSlot);
    }
}
