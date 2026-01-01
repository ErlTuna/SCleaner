using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] VoidEventChannelSO _pauseToggleEventChannel;
    [SerializeField] GameObject _backgroundDim;
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
        _pauseToggleEventChannel.OnEventRaised += TogglePauseMenu;

    }

    void OnDisable()
    {
        _pauseToggleEventChannel.OnEventRaised -= TogglePauseMenu;
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
                ToggleBackgroundDim(false);
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
        ToggleBackgroundDim(true);
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
        SettingsManager.instance.HandleSettingsReverted();
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
    void ToggleBackgroundDim(bool enable)
    {
        _backgroundDim.SetActive(enable);
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

}

    [System.Serializable]
    public enum PauseMenuSection
    {
        NONE, // Pause Menu is not active
        MAIN,       // Default Pause Menu
        SETTINGS,   // Settings Menu
        UNSAVED_WARNING,     // Unsaved changes popup
        INVENTORY
    }

    [System.Serializable]
    public enum MenuOverlay
    {
        TIPS,
        STATS,
        ACHIEVEMENTS
    }

    [System.Serializable]
    public enum MainMenuSection
    {
        NONE, // Pause Menu is not active
        MAIN,       // Default Pause Menu
        SETTINGS,   // Settings Menu
        UNSAVED_WARNING,     // Unsaved changes popup
    }

    [System.Serializable]
    public enum MenuWindowState
    {
        OPENING,
        ACTIVE,
        CLOSING,
        INACTIVE,
    }
