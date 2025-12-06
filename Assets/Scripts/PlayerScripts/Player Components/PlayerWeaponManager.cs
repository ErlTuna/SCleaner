using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [SerializeField] WeaponSwitchRequestEventChannelSO _weaponSwitchRequestChannel;
    [SerializeField] GameObject _currentWeaponGO;
    [SerializeField] PlayerWeapon _currentWeaponScript;
    //[SerializeField] Transform _weaponPosition;

    void OnEnable()
    {
        //PlayerInventoryEvents.OnWeaponsReadyEvent += ReceiveWeapon;
        //PlayerInventoryEvents.OnWeaponSwitchEvent += SwitchWeapon;
        _weaponSwitchRequestChannel.OnEventRaised += SwitchWeapon;
    }

    void OnDisable(){
        //PlayerInventoryEvents.OnWeaponsReadyEvent -= ReceiveWeapon;
        //PlayerInventoryEvents.OnWeaponSwitchEvent -= SwitchWeapon;
        _weaponSwitchRequestChannel.OnEventRaised -= SwitchWeapon;
    }

    public void Update()
    {

        if (_currentWeaponScript == null) return;

        if (PlayerInputManager.Instance.ReloadInput)
            HandleReloadInput();
        

        if (PlayerInputManager.Instance.PrimaryAttackInput)
            _currentWeaponScript.HandlePrimaryAttackInput();

        else if (PlayerInputManager.Instance.SecondaryAttackInput)
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
