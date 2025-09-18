using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;
    [SerializeField] InputActionAsset _playerActionAsset;
    [SerializeField] InputActionMap _gameplayActionMap;
    [SerializeField] InputActionMap _pauseActionMap;
    InputAction _movementInputAction;
    InputAction _pointerInputAction;
    InputAction _primaryAttackInputAction;
    InputAction _secondaryAttackInputAction;
    InputAction _dashInputAction;
    InputAction _equipmentInputAction;
    InputAction _reloadInputAction;
    InputAction _firstWeaponInputAction;
    InputAction _secondWeaponInputAction;
    InputAction _thirdWeaponInputAction;
    InputAction _fourthWeaponInputAction;
    InputAction _menuOpenAction;
    InputAction _menuCloseAction;
    InputAction _abilitySwitchAction;
    public Vector2 MovementInput;
    public Vector2 PointerInput;
    public bool AbilitySwitchInput;
    public bool PrimaryAttackInput;
    public bool IsPrimaryAttackHeld;
    public bool IsPrimaryAttackPressedThisFrame;
    public bool IsPrimaryAttackReleasedThisFrame;
    public bool SecondaryAttackInput;
    public bool DashInput;
    public bool FirstWeaponInput;
    public bool SecondWeaponInput;
    public bool ThirdWeaponInput;
    public bool FourthWeaponInput;
    public bool EquipmentInput;
    public bool ReloadInput;
    public bool MenuOpenInput;
    public bool MenuCloseInput;

    public static PlayerInputManager instance;

    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
            instance = this;

        PlayerInput = GetComponent<PlayerInput>();
        _gameplayActionMap = _playerActionAsset.FindActionMap("Gameplay");
        _gameplayActionMap.Enable();

        _movementInputAction = PlayerInput.actions["Movement"];
        _pointerInputAction = PlayerInput.actions["Pointer"];
        _primaryAttackInputAction = PlayerInput.actions["Shooting"];
        _secondaryAttackInputAction = PlayerInput.actions["Secondary Attack"];
        //_primaryAttackInputAction.performed += OnPrimaryAttackPressed;
        //_primaryAttackInputAction.canceled += OnPrimaryAttackReleased;


        _dashInputAction = PlayerInput.actions["Dash"];
        _equipmentInputAction = PlayerInput.actions["Equipment"];
        _reloadInputAction = PlayerInput.actions["Reload"];
        //_switchWeaponInputAction = PlayerInput.actions["Switch Weapons"];
        _firstWeaponInputAction = PlayerInput.actions["First Weapon"];
        _secondWeaponInputAction = PlayerInput.actions["Second Weapon"];
        _thirdWeaponInputAction = PlayerInput.actions["Third Weapon"];
        _fourthWeaponInputAction = PlayerInput.actions["Fourth Weapon"];
        _menuOpenAction = PlayerInput.actions["OpenMenu"];
        _menuCloseAction = PlayerInput.actions["CloseMenu"];
        _abilitySwitchAction = PlayerInput.actions["Switch Ability"];

    }

    void Start()
    {
        InputSystem.EnableDevice(Mouse.current);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Mouse input enabled");
    }

    void Update()
    {
        MovementInput = _movementInputAction.ReadValue<Vector2>();
        PointerInput = _pointerInputAction.ReadValue<Vector2>();
        PrimaryAttackInput = _primaryAttackInputAction.IsPressed();
        SecondaryAttackInput = _secondaryAttackInputAction.IsPressed();
        DashInput = _dashInputAction.WasPressedThisFrame();
        FirstWeaponInput = _firstWeaponInputAction.WasPressedThisFrame();
        SecondWeaponInput = _secondWeaponInputAction.WasPressedThisFrame();
        ThirdWeaponInput = _thirdWeaponInputAction.WasPressedThisFrame();
        FourthWeaponInput = _fourthWeaponInputAction.WasPressedThisFrame();
        ReloadInput = _reloadInputAction.WasPressedThisFrame();
        EquipmentInput = _equipmentInputAction.WasPressedThisFrame();
        MenuOpenInput = _menuOpenAction.WasPressedThisFrame();
        MenuCloseInput = _menuCloseAction.WasPerformedThisFrame();
        AbilitySwitchInput = _abilitySwitchAction.WasPerformedThisFrame();
    }

    public void ToggleMouseInput(bool enable)
    {
        if (enable)
        {
            InputSystem.EnableDevice(Mouse.current);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            InputSystem.DisableDevice(Mouse.current);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    void OnPrimaryAttackPressed(InputAction.CallbackContext context)
    {
        IsPrimaryAttackPressedThisFrame = true;
        IsPrimaryAttackHeld = true;               
    }

    void OnPrimaryAttackReleased(InputAction.CallbackContext context)
    {
        IsPrimaryAttackReleasedThisFrame = true;
        IsPrimaryAttackHeld = false;              


    }


}
