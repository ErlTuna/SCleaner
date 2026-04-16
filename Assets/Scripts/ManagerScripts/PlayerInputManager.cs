using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;
    [SerializeField] InputActionMap _gameplayActionMap;
    [SerializeField] InputActionMap _UIActionMap;
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
    InputAction _menuBackAction;
    InputAction _submitAction;
    InputAction _abilitySwitchAction;
    InputAction _navigationAction;
    public Vector2 MovementInput { get; private set; }
    public Vector2 PointerInput { get; private set; }
    public bool ItemPickupInput { get; private set; }
    public bool WeaponDropInput { get; private set; }
    public bool AbilitySwitchInput { get; private set; }
    public bool PrimaryAttackInput { get; private set; }
    public bool IsPrimaryAttackHeld { get; private set; }
    public bool IsPrimaryAttackPressedThisFrame { get; private set; }
    public bool IsPrimaryAttackReleasedThisFrame { get; private set; }
    public bool SecondaryAttackInput { get; private set; }
    public bool AbilityUseInput { get; private set; }
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
    public bool FirstWeaponInput { get; private set; }
    public bool SecondWeaponInput { get; private set; }
    public bool ThirdWeaponInput { get; private set; }
    public bool FourthWeaponInput { get; private set; }
    public bool EquipmentInput { get; private set; }
    public bool ReloadInput { get; private set; }
    public bool MenuOpenInput { get; private set; }
    public bool MenuBackInput { get; private set; }
    public bool SubmitPressed { get; private set; }    // single-frame press
    public bool SubmitHeld { get; private set; }     // continuous hold
    public Vector2 NavigationInput {get; private set; }

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
        _menuBackAction = PlayerInput.currentActionMap.FindAction("Back");
        _submitAction = PlayerInput.currentActionMap.FindAction("Submit");
        _navigationAction = PlayerInput.currentActionMap.FindAction("Navigate");


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
        //Debug.Log("Pointer input value : " + PointerInput);
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
        MenuBackInput = _menuBackAction.WasPressedThisFrame();
        AbilitySwitchInput = _abilitySwitchAction.WasPerformedThisFrame();
        SubmitPressed = _submitAction.WasPressedThisFrame();
        SubmitHeld = _submitAction.ReadValue<float>() > 0;
        NavigationInput = _navigationAction.ReadValue<Vector2>();

    }

    //public static event Action<Vector2> OnUINavigation;


    public void ToggleMouseInput(bool enable)
    {
        
        if (enable)
        {
            InputSystem.EnableDevice(Mouse.current);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Mouse input enabled.");
        }
        else
        {
            Debug.Log("Mouse input disabled.");
            InputSystem.DisableDevice(Mouse.current);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
    }

    public void SwitchToGameplayActionMap()
    {
        //Debug.Log("Switching to gameplay action map.");
        PlayerInput.SwitchCurrentActionMap("Gameplay");
    }

    public void SwitchToUIActionMap()
    {
        //Debug.Log("Switching to UI action map.");
        PlayerInput.SwitchCurrentActionMap("UI");
    }

    public void OnPlayerDefeat()
    {
        if (_UIActionMap == null) return;
        if (_gameplayActionMap == null) return;
        _gameplayActionMap.Disable();
    }

}
