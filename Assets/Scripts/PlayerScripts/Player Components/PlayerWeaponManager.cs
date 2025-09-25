using System.Collections;
using System.Collections.Generic;
using SerializeReferenceEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] GameObject _currentWeaponGO;
    public Transform _weaponPosition;
    public AudioClip weaponSwitchAudio;
    public BaseWeapon currentWeaponScript;

    void OnEnable()
    {
        PlayerInventoryEvents.OnInventoryReadyEvent += ReceiveWeapon;
        PlayerInventoryEvents.OnWeaponSwitchEvent += SwitchWeapon;
    }

    void OnDisable(){
        PlayerInventoryEvents.OnInventoryReadyEvent -= ReceiveWeapon;
        PlayerInventoryEvents.OnWeaponSwitchEvent -= SwitchWeapon;
    }

    public void Update()
    {

        if (currentWeaponScript == null) return;

        if (PlayerInputManager.instance.ReloadInput)
            HandleReloadInput();
        

        if (PlayerInputManager.instance.PrimaryAttackInput)
            currentWeaponScript.HandlePrimaryAttackInput();

        else if (PlayerInputManager.instance.SecondaryAttackInput)
            currentWeaponScript.HandleSecondaryAttackInput();


        else
            currentWeaponScript.HandlePrimaryAttackInputCancel();
    }

    public void ReceiveWeapon(GameObject weapon, BaseWeapon weaponScript)
    {
        _currentWeaponGO = weapon;
        currentWeaponScript = weaponScript;
        _currentWeaponGO.SetActive(true);
    }

    
    public void SwitchWeapon(GameObject targetWeapon, BaseWeapon targetWeaponScript){
        

        PlayPlayerSounds.PlayAudio(weaponSwitchAudio);
        currentWeaponScript.SwitchFrom();

        _currentWeaponGO = targetWeapon;
        currentWeaponScript = targetWeaponScript;

        _currentWeaponGO.SetActive(true);
    }

    public void HandleReloadInput(){
        currentWeaponScript.HandleReloadInput();
    }
}
