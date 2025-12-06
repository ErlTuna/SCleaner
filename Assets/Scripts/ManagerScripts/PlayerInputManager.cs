using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;
    //[SerializeField] InputActionAsset _playerActionAsset;
    [SerializeField] InputActionMap _gameplayActionMap;
    [SerializeField] InputActionMap _UIActionMap;
    InputActionMap _previousActionMap;

    InputAction _movementInputAction;
    InputAction _pointerInputAction;
    InputAction _itemPickupInputAction;
    InputAction _weaponDropInputAction;
    InputAction _primaryAttackInputAction;
    InputAction _secondaryAttackInputAction;
    InputAction _abilityInputAction;
    InputAction _equipmentInputAction;
    InputAction _reloadInputAction;
    InputAction _weaponSwitchInputAction;
    InputAction _firstWeaponInputAction;
    InputAction _secondWeaponInputAction;
    InputAction _thirdWeaponInputAction;
    InputAction _fourthWeaponInputAction;
    InputAction _menuOpenAction;
    InputAction _menuCloseAction;
    InputAction _abilitySwitchAction;
    public Vector2 MovementInput;
    public Vector2 PointerInput;
    public bool ItemPickupInput;
    public bool WeaponDropInput;
    public bool AbilitySwitchInput;
    public bool PrimaryAttackInput;
    public bool IsPrimaryAttackHeld;
    public bool IsPrimaryAttackPressedThisFrame;
    public bool IsPrimaryAttackReleasedThisFrame;
    public bool SecondaryAttackInput;
    public bool AbilityUseInput;
    public int WeaponSwitchInput
        {
            get
            {
                if (_weaponSwitchInputAction.triggered)
                {
                    if (_weaponSwitchInputAction.activeControl is KeyControl keyControl)
                    {
                        switch (keyControl.keyCode)
                        {
                            case Key.Digit1: return 0;
                            case Key.Digit2: return 1;
                            case Key.Digit3: return 2;
                            case Key.Digit4: return 3;
                            default: return -1;
                        }
                    }
                }
                return -1;
            }
        }
    public bool FirstWeaponInput;
    public bool SecondWeaponInput;
    public bool ThirdWeaponInput;
    public bool FourthWeaponInput;
    public bool EquipmentInput;
    public bool ReloadInput;
    public bool MenuOpenInput;
    public bool MenuCloseInput;

    public static PlayerInputManager Instance {get; private set;}

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
            Instance = this;

        PlayerInput = GetComponent<PlayerInput>();
        //_gameplayActionMap = _playerActionAsset.FindActionMap("Gameplay");
        //_UIActionMap = _playerActionAsset.FindActionMap("PauseMenu");
        _gameplayActionMap = PlayerInput.actions.FindActionMap("Gameplay");
        _UIActionMap = PlayerInput.actions.FindActionMap("UI");
        PlayerInput.SwitchCurrentActionMap("Gameplay");
        //_gameplayActionMap.Enable();

        _movementInputAction = PlayerInput.actions["Movement"];
        _pointerInputAction = PlayerInput.actions["Pointer"];
        _primaryAttackInputAction = PlayerInput.actions["Shooting"];
        _secondaryAttackInputAction = PlayerInput.actions["Secondary Attack"];


        _itemPickupInputAction = PlayerInput.actions["Pick Up Item"];
        _weaponDropInputAction = PlayerInput.actions["Drop Weapon"];
        _abilityInputAction = PlayerInput.actions["Ability"];
        _equipmentInputAction = PlayerInput.actions["Equipment"];
        _reloadInputAction = PlayerInput.actions["Reload"];

        _weaponSwitchInputAction = PlayerInput.actions["Weapon Switch"];
        _firstWeaponInputAction = PlayerInput.actions["First Weapon"];
        _secondWeaponInputAction = PlayerInput.actions["Second Weapon"];
        _thirdWeaponInputAction = PlayerInput.actions["Third Weapon"];
        _fourthWeaponInputAction = PlayerInput.actions["Fourth Weapon"];
        _menuOpenAction = PlayerInput.actions["Open Menu"];
        
        _abilitySwitchAction = PlayerInput.actions["Switch Ability"];

        // Switching to UI map to cache the CloseMenu action
        PlayerInput.SwitchCurrentActionMap("UI");
        _menuCloseAction = PlayerInput.actions["CloseMenu"];

        PlayerInput.SwitchCurrentActionMap("Gameplay");


    }

    void Start()
    {
        ToggleMouseInput(true);
    }

    void Update()
    {
        MovementInput = _movementInputAction.ReadValue<Vector2>();
        PointerInput = _pointerInputAction.ReadValue<Vector2>();
        ItemPickupInput = _itemPickupInputAction.WasPressedThisFrame();
        WeaponDropInput = _weaponDropInputAction.WasPressedThisFrame();
        
        PrimaryAttackInput = _primaryAttackInputAction.IsPressed();
        SecondaryAttackInput = _secondaryAttackInputAction.IsPressed();
        AbilityUseInput = _abilityInputAction.WasPressedThisFrame();
        FirstWeaponInput = _firstWeaponInputAction.WasPressedThisFrame();
        SecondWeaponInput = _secondWeaponInputAction.WasPressedThisFrame();
        ThirdWeaponInput = _thirdWeaponInputAction.WasPressedThisFrame();
        FourthWeaponInput = _fourthWeaponInputAction.WasPressedThisFrame();
        ReloadInput = _reloadInputAction.WasPressedThisFrame();
        EquipmentInput = _equipmentInputAction.IsPressed();
        MenuOpenInput = _menuOpenAction.WasPressedThisFrame();
        MenuCloseInput = _menuCloseAction.WasPressedThisFrame();
        AbilitySwitchInput = _abilitySwitchAction.WasPerformedThisFrame();
        
    }

    public void ToggleMouseInput(bool enable)
    {
        
        if (enable)
        {
            InputSystem.EnableDevice(Mouse.current);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Mouse input enabled");
        }
        else
        {
            InputSystem.DisableDevice(Mouse.current);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }

    public void SwitchToGameplayActionMap()
    {
        //_previousActionMap = _UIActionMap;
        //_UIActionMap.Disable();
        PlayerInput.SwitchCurrentActionMap("Gameplay");
    }

    public void SwitchToUIActionMap()
    {
        //_previousActionMap = _gameplayActionMap;
        PlayerInput.SwitchCurrentActionMap("UI");
        //_UIActionMap.Enable();
    }

    public void OnPlayerDefeat()
    {
        if (_UIActionMap == null) return;
        if (_gameplayActionMap == null) return;
            
        _gameplayActionMap.Disable();
        _UIActionMap.Enable();
        Debug.Log("Switched input action maps.");
    }

}
