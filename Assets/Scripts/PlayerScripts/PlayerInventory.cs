using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerInventory : MonoBehaviour
{

    public static Action<GameObject, IWeapon, WeaponSO> OnInventoryReadyEvent;
    public static Action<GameObject, IWeapon> OnWeaponSwitchEvent;
    [SerializeField] List<GameObject> weaponPrefabs = new();
    [SerializeField] List<GameObject> weaponGOs = new();
    [SerializeField] List<IWeapon> weaponScripts = new();
    [SerializeField] Transform _weaponParent;
    [SerializeField] Transform _weaponPosition;
    GameObject currentWeapon;
    IWeapon currentWeaponScript;
    int weaponCount = 4;
    public int currentlyUsedSlot = 0;

    void Awake(){
        GameObject instantiatedWeaponPrefab;
        for(int i = 0; i < weaponCount; ++i){
            if(weaponPrefabs.ElementAt(i)){
                instantiatedWeaponPrefab = Instantiate(weaponPrefabs[i]);
                weaponGOs.Add(instantiatedWeaponPrefab);
            }
        }
    }
    
    void Start()
    {
        IWeapon instantiatedWeaponScript;
        for(int i = 0; i < weaponCount; ++i){
            if(_weaponParent != null){

            instantiatedWeaponScript = weaponGOs.ElementAt(i).GetComponent<IWeapon>();
            weaponScripts.Add(instantiatedWeaponScript);
            weaponGOs[i].transform.parent = _weaponParent;
            //print(_weaponPosition.position);
            weaponGOs[i].transform.position = _weaponPosition.position;

            weaponGOs[i].transform.localPosition += instantiatedWeaponScript.WeaponInfo.offset;
            weaponGOs[i].SetActive(false);
            }
            
        }
        currentWeapon = weaponGOs.ElementAt(currentlyUsedSlot);
        currentWeaponScript = weaponScripts.ElementAt(currentlyUsedSlot);
        OnInventoryReadyEvent?.Invoke(currentWeapon, currentWeaponScript, currentWeaponScript.WeaponInfo);
    }

    void Update(){
        if (PlayerInputManager.instance.FirstWeaponInput)
            SwitchWeapons(0);
        else if (PlayerInputManager.instance.SecondWeaponInput)
            SwitchWeapons(1);
        else if (PlayerInputManager.instance.ThirdWeaponInput)
            SwitchWeapons(2);
        else if (PlayerInputManager.instance.FourthWeaponInput)
            SwitchWeapons(3);
    }
    /*public void WeaponSwap(InputAction.CallbackContext ctx){

        if (currentWeaponScript.WeaponInfo.isFiring) return;

        int slotToSwitchTo;
        if (ctx.performed)
        {
            if (ctx.control is KeyControl keyControl)
            {
                Key pressedKey = keyControl.keyCode;
                switch (pressedKey)
                {
                    case Key.Digit1:
                        slotToSwitchTo = 0;
                        if (weaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                        {
                            WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                        }

                        break;
                    case Key.Digit2:
                        slotToSwitchTo = 1;
                        if (weaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                        {
                            WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                        }

                        break;
                    case Key.Digit3:
                        slotToSwitchTo = 2;
                        if (weaponGOs[slotToSwitchTo] != null && slotToSwitchTo != currentlyUsedSlot)
                        {
                            WeaponSlotChange(currentlyUsedSlot, slotToSwitchTo);
                        }
                        break;
                    case Key.Digit4:
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
        }

    }*/

    void SwitchWeapons(int slot){
        if (currentWeaponScript.WeaponInfo.isFiring) return;

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

    void WeaponSlotChange(int currentSlot, int slotToSwitchTo){
        OnWeaponSwitchEvent?.Invoke(weaponGOs[slotToSwitchTo], weaponScripts[slotToSwitchTo]);
        currentWeaponScript = weaponScripts[slotToSwitchTo];
        currentlyUsedSlot = slotToSwitchTo;
    }
}
