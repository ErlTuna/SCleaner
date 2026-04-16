using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour, IPickupHandler
{
    [SerializeField] WeaponSwitchRequestEventChannelSO _weaponSwitchRequestChannel;
    [SerializeField] GameObject _currentWeaponGO;
    [SerializeField] PlayerWeapon _currentWeaponScript;

    // Public Getters
    public PlayerWeapon CurrentWeaponScript => _currentWeaponScript;

    void Awake()
    {
        _weaponSwitchRequestChannel.OnEventRaised += SwitchWeapon;
    }

    void OnDisable(){

        _weaponSwitchRequestChannel.OnEventRaised -= SwitchWeapon;
    }

    public void Update()
    {

        if (_currentWeaponScript == null) return;

        if (PlayerInputManager.Instance.ReloadInput)
            HandleReloadInput();
        

        if (PlayerInputManager.Instance.PrimaryAttackInput)
        {
            _currentWeaponScript.HandlePrimaryAttackInput();
        }
        else
            _currentWeaponScript.HandlePrimaryAttackInputRelease();
    }
    
    public void SwitchWeapon(GameObject targetWeapon, PlayerWeapon targetWeaponScript)
    {        
        if (_currentWeaponScript)
            _currentWeaponScript.SwitchFrom();

        Debug.Log("SWITCHING TO WEAPON :" + targetWeaponScript.name);
        _currentWeaponGO = targetWeapon;
        _currentWeaponScript = targetWeaponScript;

        if(_currentWeaponGO)
            _currentWeaponGO.SetActive(true);
    }

    public void HandleReloadInput()
    {
        _currentWeaponScript.HandleReloadInput();
    }
}
