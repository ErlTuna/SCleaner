using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MenuContext
{
    [Header("Sections")]
    [SerializeField] MainMenuMainSection _mainSection;
    [SerializeField] MainMenuHowToPlaySection _howToPlaySection;
    [SerializeField] SettingsMenu _settingsSection;
    [SerializeField] WarningPopUp _warningPopUpGO;

    [SerializeField] MainMenuDefaultsSO _defaultSettingsSO;
    [SerializeField] MainMenuSection _currentSection = MainMenuSection.NONE;
    [SerializeField] MenuWindowState _currentWindowState = MenuWindowState.INACTIVE;

    Dictionary<MainMenuSection, IMenuSection> _sections;


    void Awake()
    {
        _defaultSettingsSO = LoadOrCloneSettings(_defaultSettingsSO);
        _sections = new()
        {
            { MainMenuSection.MAIN, _mainSection },
            { MainMenuSection.SETTINGS, _settingsSection },
            { MainMenuSection.UNSAVED_WARNING, _warningPopUpGO },
            { MainMenuSection.HOW_TO_PLAY, _howToPlaySection }
        };
    }

    void Start()
    {
        SETTINGS.Initialize(_defaultSettingsSO);
        SetWindowState(MenuWindowState.OPENING);
        SetCurrentContext(this);
    }

    void Update()
    {
        if (PlayerInputManager.Instance.MenuBackInput)
            Execute(MenuAction.Back);
    }

    public void SetWindowState(MenuWindowState newState)
    {
        _currentWindowState = newState;

        switch (_currentWindowState)
        {            
            case MenuWindowState.OPENING:
                Debug.Log("Main Menu window is opening.");
                SetSection(MainMenuSection.MAIN);
                SetWindowState(MenuWindowState.ACTIVE);
                break;

            case MenuWindowState.ACTIVE:
                Debug.Log("Main menu window is open.");
                break;

            case MenuWindowState.CLOSING:
                Debug.Log("Main Menu window is closing.");
                SetWindowState(MenuWindowState.INACTIVE);
                break;

            case MenuWindowState.INACTIVE:
                SetSection(MainMenuSection.NONE);
                UISelector.instance.SetSelected(null);
                Debug.Log("Main Menu window is closed.");
                break;
        }

        
    }

    public void SetSection(MainMenuSection newSection)
    {
        if (_currentSection != MainMenuSection.NONE)
            _sections[_currentSection].Hide();

        _currentSection = newSection;

        if (_currentSection != MainMenuSection.NONE)
            _sections[_currentSection].Show();
    }


    MainMenuDefaultsSO LoadOrCloneSettings(MainMenuDefaultsSO settings)
    {
        if (settings == null) return Instantiate(Resources.Load<MainMenuDefaultsSO>("ScriptableObjects/MainMenuDefaultSettingsSO"));

        else return Instantiate(settings);
    }

    public override void Execute(MenuAction action)
    {
        switch (action)
        {
            case MenuAction.StartGame:
                GameManager.Instance.SetGameState(GameState.LOADING_GAME);
                break;

            case MenuAction.OpenSettings:
                SetSection(MainMenuSection.SETTINGS);
                break;

            case MenuAction.Back:
                HandleBack();
                break;

            case MenuAction.ApplySettings:
                SettingsManager.Instance.HandleSettingsSaved();
                break;

            case MenuAction.ShowUnsavedWarning:
                SetSection(MainMenuSection.UNSAVED_WARNING);
                break;

            case MenuAction.WarningConfirmYes:
                OnWarningYes();
                break;

            case MenuAction.WarningConfirmNo:
                OnWarningNo();
                break;
            
            case MenuAction.OpenHowToPlaySection:
                SetSection(MainMenuSection.HOW_TO_PLAY);
                break;

            case MenuAction.QuitGame:
                GameManager.Instance.SetGameState(GameState.SHUTTING_DOWN);
                break;

            default :
                Debug.Log("The action is not supported for this context.");
                break;
        }
    }

    void OnWarningYes()
    {
        SettingsManager.Instance.HandleSettingsReverted();
        SetSection(MainMenuSection.MAIN);
    }

    void OnWarningNo()
    {
        SetSection(MainMenuSection.SETTINGS);
    }

    void HandleBack()
    {
        switch (_currentSection)
        {
            case MainMenuSection.SETTINGS:
                if (SettingsManager.Instance.CheckIfDirty())
                {
                    SetSection(MainMenuSection.UNSAVED_WARNING);
                }
                else
                {
                    SetSection(MainMenuSection.MAIN);
                }
                break;

            default:
                SetSection(MainMenuSection.MAIN);
                break;
        }
    }

}
