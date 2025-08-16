using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InGameMenuManager : MonoBehaviour
{
    [SerializeField] GameObject _backgroundDim;
    [SerializeField] GameObject _pauseMenuGO;
    [SerializeField] GameObject _settingsMenuGO;
    [SerializeField] GameObject _warningPopUpGO;
    private List<GameObject> Menus = new();

    void OnEnable()
    {
        MenuOpenCloseEvents.OnPauseMenuClosed += TogglePauseMenu;
        MenuOpenCloseEvents.OnSettingsOpened += ToggleSettingsMenu;
        MenuOpenCloseEvents.OnSettingsClosed += TogglePauseMenu;
        SettingsEvents.OnSettingsChangesUnsaved += ToggleWarningPopup;
        PopUpEvents.OnWarningYes += TogglePauseMenu;
        PopUpEvents.OnWarningNo += ToggleSettingsMenu;
    }

    void OnDisable()
    {
        MenuOpenCloseEvents.OnPauseMenuClosed -= TogglePauseMenu;
        MenuOpenCloseEvents.OnSettingsOpened -= ToggleSettingsMenu;
        MenuOpenCloseEvents.OnSettingsClosed -= TogglePauseMenu;
        SettingsEvents.OnSettingsChangesUnsaved -= ToggleWarningPopup;
        PopUpEvents.OnWarningYes -= TogglePauseMenu;
        PopUpEvents.OnWarningNo -= ToggleSettingsMenu;
    }

    void Start()
    {
        //Menus.Add(_pauseMenuGO);
        //Menus.Add(_settingsMenuGO);
    }


    void Update()
    {
        if (PlayerInputManager.instance.MenuOpenInput)
        {
            if (!PauseManager.instance.IsPaused)
            {
                PlayerInputManager.instance.ToggleMouseInput(false);
                HandlePauseInput();
            }
        }

        else if (PlayerInputManager.instance.MenuCloseInput && _pauseMenuGO.activeSelf)
        {
            if (PauseManager.instance.IsPaused)
            {
                PlayerInputManager.instance.ToggleMouseInput(true);
                HandleUnPauseInput();
            }
        }
    }

    void HandlePauseInput()
    {
        
        TogglePauseMenu();
        PauseManager.instance.PauseGame();
    }

    public void HandleUnPauseInput()
    {
        
        TogglePauseMenu();
        PauseManager.instance.UnPauseGame();
    }

    void TogglePauseMenu()
    {
        if (_settingsMenuGO && _settingsMenuGO.activeSelf)
            _settingsMenuGO.SetActive(false);

        if (_warningPopUpGO && _warningPopUpGO.activeSelf)
            _warningPopUpGO.SetActive(false);


        Debug.Log(_pauseMenuGO);
        bool isActive = !_pauseMenuGO.activeSelf;
        ToggleBackgroundDim(isActive);
        _pauseMenuGO.SetActive(isActive);
        if (!isActive)
            UISelector.instance.SetSelected(null);
    }

    void ToggleSettingsMenu()
    {
        if(_pauseMenuGO && _pauseMenuGO.activeSelf)
            _pauseMenuGO.SetActive(false);

        if (_warningPopUpGO && _warningPopUpGO.activeSelf)
            _warningPopUpGO.SetActive(false);

        bool isActive = !_settingsMenuGO.activeSelf;
        _settingsMenuGO.SetActive(isActive);
    }

    void ToggleWarningPopup()
    {
        if (_settingsMenuGO && _settingsMenuGO.activeSelf)
            _settingsMenuGO.SetActive(false);

        bool isActive = !_warningPopUpGO.activeSelf;
        _warningPopUpGO.SetActive(isActive);
    }

    void ToggleBackgroundDim(bool enable)
    {
        _backgroundDim.SetActive(enable);
    }

    void CloseAllMenus()
    {
        foreach (GameObject menu in Menus)
        {
            menu.SetActive(false);
        }
    }


    
}
