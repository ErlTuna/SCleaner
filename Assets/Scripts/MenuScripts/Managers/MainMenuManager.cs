using UnityEngine;
using UnityEngine.InputSystem;


// script responsible for managing the main menu "windows"
// such as the main menu itself, the settings menu and the warning pop-up upon unsaved changes
// the menu flow is simple enough that this solution works and is enough for what is aimed to be achieved
// tl;dr handles the menu flow
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] MainMenuDefaultsSO _defaultSettingsSO;
    [SerializeField] GameObject _mainMenuGO;
    [SerializeField] GameObject _settingsMenuGO;
    [SerializeField] GameObject _warningPopupGO;
    [SerializeField] bool _enableMouseInput = true;

    void OnEnable()
    {
        MenuOpenCloseEvents.OnSettingsOpened += ToggleSettingsMenu;
        MenuOpenCloseEvents.OnSettingsClosed += ToggleMainMenu;

        SettingsEvents.OnSettingsChangesUnsaved += ToggleSettingsMenu;

        PopUpEvents.OnPopUpTriggered += ToggleWarningPopup;
        PopUpEvents.OnWarningYes += ToggleMainMenu;
        PopUpEvents.OnWarningNo += ToggleSettingsMenu;
    }

    void OnDisable()
    {
        MenuOpenCloseEvents.OnSettingsOpened -= ToggleSettingsMenu;
        MenuOpenCloseEvents.OnSettingsClosed -= ToggleMainMenu;

        SettingsEvents.OnSettingsChangesUnsaved -= ToggleSettingsMenu;

        PopUpEvents.OnPopUpTriggered -= ToggleWarningPopup;
        PopUpEvents.OnWarningYes -= ToggleMainMenu;
        PopUpEvents.OnWarningNo -= ToggleSettingsMenu;
    }

    void Awake()
    {
        _defaultSettingsSO = LoadOrCloneSettings(_defaultSettingsSO);

    }

    void Start()
    {
        SETTINGS.Initialize(_defaultSettingsSO);
        ToggleMouseInput(false);
    }

    void ToggleMouseInput(bool enable)
    {
        if (enable)
        {
            InputSystem.EnableDevice(Mouse.current);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Mouse input enabled");
        }
        else
        {
            InputSystem.DisableDevice(Mouse.current);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("Mouse input disabled");
        }
    }

    MainMenuDefaultsSO LoadOrCloneSettings(MainMenuDefaultsSO settings)
    {
        if (settings == null) return Instantiate(Resources.Load<MainMenuDefaultsSO>("ScriptableObjects/MainMenuDefaultSettingsSO"));

        else return Instantiate(settings);
    }

    void ToggleMainMenu()
    {
        if (_settingsMenuGO && _settingsMenuGO.activeSelf)
            _settingsMenuGO.SetActive(false);

        if (_warningPopupGO && _warningPopupGO.activeSelf)
            _warningPopupGO.SetActive(false);

        bool isActive = !_mainMenuGO.activeSelf;
        _mainMenuGO.SetActive(isActive);
    }

    void ToggleSettingsMenu()
    {
        if (_mainMenuGO && _mainMenuGO.activeSelf)
            _mainMenuGO.SetActive(false);

        if (_warningPopupGO && _warningPopupGO.activeSelf)
            _warningPopupGO.SetActive(false);

        bool isActive = !_settingsMenuGO.activeSelf;
        _settingsMenuGO.SetActive(isActive);
    }

    void ToggleWarningPopup()
    {
        if (_settingsMenuGO && _settingsMenuGO.activeSelf)
            _settingsMenuGO.SetActive(false);

        bool isActive = !_warningPopupGO.activeSelf;
        _warningPopupGO.SetActive(isActive);
    }
}

