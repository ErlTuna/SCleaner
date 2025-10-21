using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] GameObject _currentWeaponGO;
    [SerializeField] PlayerWeapon _currentWeaponScript;
    //[SerializeField] Transform _weaponPosition;

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

        if (_currentWeaponScript == null) return;

        if (PlayerInputManager.instance.ReloadInput)
            HandleReloadInput();
        

        if (PlayerInputManager.instance.PrimaryAttackInput)
            _currentWeaponScript.HandlePrimaryAttackInput();

        else if (PlayerInputManager.instance.SecondaryAttackInput)
            _currentWeaponScript.HandleSecondaryAttackInput();


        else
            _currentWeaponScript.HandlePrimaryAttackInputCancel();
    }

    public void ReceiveWeapon(GameObject weapon, PlayerWeapon weaponScript)
    {
        _currentWeaponGO = weapon;
        _currentWeaponScript = weaponScript;
        _currentWeaponGO.SetActive(true);
    }

    
    public void SwitchWeapon(GameObject targetWeapon, PlayerWeapon targetWeaponScript){        
        if (_currentWeaponScript)
            _currentWeaponScript.SwitchFrom();

        _currentWeaponGO = targetWeapon;
        _currentWeaponScript = targetWeaponScript;

        if(_currentWeaponGO)
            _currentWeaponGO.SetActive(true);
    }

    public void HandleReloadInput(){
        _currentWeaponScript.HandleReloadInput();
    }
}
