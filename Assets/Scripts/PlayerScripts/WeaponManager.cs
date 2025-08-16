using UnityEngine;

    //This class handles input related to the weapon functionality
    //such as shooting, reloading etc. It only takes the input and
    //relegates the actual functionality to the weapon itself

public class WeaponManager : MonoBehaviour
{
    public Sprite playerAttackSprite;
    public Sprite defaultSprite;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _currentWeaponGO;
    public Transform _weaponPosition;
    public AudioClip weaponSwitchAudio;
    public IWeapon currentWeaponScript;
    
    bool _isLMBHeld;

    void OnEnable(){
        PlayerInventory.OnInventoryReadyEvent += ReceiveWeapon;
        PlayerInventory.OnWeaponSwitchEvent += SwitchWeapon;
    }

    void OnDisable(){
        PlayerInventory.OnInventoryReadyEvent -= ReceiveWeapon;
        PlayerInventory.OnWeaponSwitchEvent -= SwitchWeapon;
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
            _spriteRenderer.sprite = playerAttackSprite;
        }
        else if (_isLMBHeld && !PlayerInputManager.instance.PrimaryAttackInput)
        {
            _spriteRenderer.sprite = defaultSprite;
            _isLMBHeld = false;
            currentWeaponScript.HandlePrimaryAttackInputCancel();
        }
        
    }

    public void ReceiveWeapon(GameObject weapon, IWeapon weaponScript, WeaponSO info){
        _currentWeaponGO = weapon;
        currentWeaponScript = weaponScript;
        if(currentWeaponScript.WeaponInfo == null)
            currentWeaponScript.WeaponInfo = info;
        
        _currentWeaponGO.SetActive(true);
    }

    
    public void SwitchWeapon(GameObject weapon, IWeapon weaponScript){
        if(currentWeaponScript.WeaponInfo.isFiring) return;
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

    /*
    public void ReceivePrimaryAttackInputAction(InputAction action)
    {
        _primaryAttackAction = action;
    }
    */

    
}