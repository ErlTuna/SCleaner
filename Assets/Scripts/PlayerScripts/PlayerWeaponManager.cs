using System.Collections;
using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] GameObject _currentWeaponGO;
    public Transform _weaponPosition;
    public AudioClip weaponSwitchAudio;
    public BaseWeapon currentWeaponScript;
    
    bool _isLMBHeld;

    void OnEnable(){
        PlayerInventoryManager.OnInventoryReadyEvent += ReceiveWeapon;
        PlayerInventoryManager.OnWeaponSwitchEvent += SwitchWeapon;
    }

    void OnDisable(){
        PlayerInventoryManager.OnInventoryReadyEvent -= ReceiveWeapon;
        PlayerInventoryManager.OnWeaponSwitchEvent -= SwitchWeapon;
    }

    public void Update(){

        if (PlayerInputManager.instance.ReloadInput)
        {
            HandleReloadInput();
        }


        if (PlayerInputManager.instance.PrimaryAttackInput)
        {
            _isLMBHeld = true;
            currentWeaponScript.HandlePrimaryAttackInput();
        }
        else if (_isLMBHeld && !PlayerInputManager.instance.PrimaryAttackInput)
        {
            _isLMBHeld = false;
            currentWeaponScript.HandlePrimaryAttackInputCancel();
        }
        
    }

    public void ReceiveWeapon(GameObject weapon, BaseWeapon weaponScript, WeaponData info){
        _currentWeaponGO = weapon;
        currentWeaponScript = weaponScript;
        if(currentWeaponScript.data == null)
            currentWeaponScript.data = info;
        
        _currentWeaponGO.SetActive(true);
    }

    
    public void SwitchWeapon(GameObject weapon, BaseWeapon weaponScript){
        if(currentWeaponScript.data.state == WeaponState.FIRING) return;
        PlayPlayerSounds.PlayAudio(weaponSwitchAudio);
        weaponScript.ResetWeaponState();
        _currentWeaponGO.SetActive(false);
        _currentWeaponGO = weapon;
        _currentWeaponGO.SetActive(true);
        currentWeaponScript = weaponScript;
    }

    public void HandleReloadInput(){
        currentWeaponScript.HandleReloadStart();
    }
}
