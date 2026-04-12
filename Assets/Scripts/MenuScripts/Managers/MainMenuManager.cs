using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

using Image = UnityEngine.UI.Image;
using System.Collections.Generic;

// script responsible for managing the main menu "windows"
// such as the main menu itself, the settings menu and the warning pop-up upon unsaved changes
// the menu flow is simple enough that this solution works and is enough for what is aimed to be achieved
// tl;dr handles the menu flow
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject _initialSelection;
    [SerializeField] Image _initialSelectionIndicator;
    [SerializeField] MainMenuDefaultsSO _defaultSettingsSO;
    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _pauseMenuGO;
    [SerializeField] GameObject _pauseMenuFirstSelected;
    [SerializeField] GameObject _settingsMenuGO;
    [SerializeField] GameObject _settingsMenuFirstSelected;
    [SerializeField] GameObject _warningPopUpGO;
    [SerializeField] GameObject _warningMenuFirstSelected;
    [SerializeField] List<GameObject> _menus = new();
    [SerializeField] PauseMenuSection _currentSection = PauseMenuSection.NONE;
    [SerializeField] MenuWindowState _currentWindowState = MenuWindowState.INACTIVE;

    void OnEnable()
    {
        UISelector.instance.SetSelected(_initialSelection);
        MenuAnimationsManager.instance.OptionSelected(_initialSelectionIndicator);
    }

    void Awake()
    {
        _defaultSettingsSO = LoadOrCloneSettings(_defaultSettingsSO);

    }

    void Start()
    {
        SETTINGS.Initialize(_defaultSettingsSO);
    }

    public void SetWindowState(MenuWindowState newState)
    {
        _currentWindowState = newState;

        switch (_currentWindowState)
        {            
            case MenuWindowState.OPENING:
                Debug.Log("Pause Menu window is opening.");
                ToggleCanvas(true);
                SetSection(PauseMenuSection.MAIN);
                SetWindowState(MenuWindowState.ACTIVE);
                break;

            case MenuWindowState.ACTIVE:
                Debug.Log("Pause menu window is open.");
                break;

            case MenuWindowState.CLOSING:
                Debug.Log("Pause Menu window is closing.");
                SetWindowState(MenuWindowState.INACTIVE);
                break;

            case MenuWindowState.INACTIVE:
                ToggleCanvas(false);
                CloseAllMenuItems();
                SetSection(PauseMenuSection.NONE);
                UISelector.instance.SetSelected(null);
                Debug.Log("Pause Menu window is closed.");
                break;
        }

        
    }

    public void SetSection(PauseMenuSection newState)
    {
        _currentSection = newState;

        switch (_currentSection)
            {
                case PauseMenuSection.MAIN:
                    OnEnterMain();
                    break;
                case PauseMenuSection.SETTINGS:
                    OnEnterSettings();
                    break;
                case PauseMenuSection.UNSAVED_WARNING:
                    OnShowWarning();
                    break;
            }
    }

    void TogglePauseMenu()
    {
        if (_currentWindowState == MenuWindowState.INACTIVE)
        {
            SetWindowState(MenuWindowState.OPENING);
        }
            
        else if (_currentWindowState != MenuWindowState.INACTIVE)
        {
            SetWindowState(MenuWindowState.CLOSING);
        }
    }

    // ------------------------
    // TRANSITIONS
    // ------------------------

    // These are exposed for UnityEvents
    public void GoToSettings() => SetSection(PauseMenuSection.SETTINGS);
    public void GoToMain() => SetSection(PauseMenuSection.MAIN);
    public void ShowWarning() => SetSection(PauseMenuSection.UNSAVED_WARNING);

    void OnEnterMain()
    {
        _settingsMenuGO.SetActive(false);
        _pauseMenuGO.SetActive(true);
        UISelector.instance.SetSelected(_pauseMenuFirstSelected);
    }

    void OnEnterSettings()
    {
        _pauseMenuGO.SetActive(false);
        _settingsMenuGO.SetActive(true);
        UISelector.instance.SetSelected(_settingsMenuFirstSelected);
    }

    void OnShowWarning()
    {
        _settingsMenuGO.SetActive(false);
        _warningPopUpGO.SetActive(true);
        UISelector.instance.SetSelected(_warningMenuFirstSelected);
    }

    // These two methods are exposed for UnityEvents
    public void OnWarningNo()
    {
        _warningPopUpGO.SetActive(false);
        _settingsMenuGO.SetActive(true);
        UISelector.instance.SetSelected(_settingsMenuFirstSelected);
    }

    public void OnWarningYes()
    {
        SettingsManager.Instance.HandleSettingsReverted();
        _warningPopUpGO.SetActive(false);
        _pauseMenuGO.SetActive(true);
        UISelector.instance.SetSelected(_pauseMenuFirstSelected);
    }

    // ----------------------------
    // HELPERS
    // ----------------------------
    void ToggleCanvas(bool enable)
    {
        _canvas.enabled = enable;
    }
    void EnableAllMenuItems()
    {
        foreach (GameObject menu in _menus)
        {
            menu.SetActive(true);
        }
    }

    void CloseAllMenuItems()
    {
        foreach (GameObject menu in _menus)
        {
            menu.SetActive(false);
        }
    }

    MainMenuDefaultsSO LoadOrCloneSettings(MainMenuDefaultsSO settings)
    {
        if (settings == null) return Instantiate(Resources.Load<MainMenuDefaultsSO>("ScriptableObjects/MainMenuDefaultSettingsSO"));

        else return Instantiate(settings);
    }


}

