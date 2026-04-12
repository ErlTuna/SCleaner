using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MenuContext
{
    [SerializeField] GameStateEventChannel _gameStateChangedEventChannel;

    [Header("Sections")]
    [SerializeField] PauseMenuMainSection _mainSection;
    //[SerializeField] PauseMenuSettingsSection _settingsSection;
    [SerializeField] SettingsMenu _settingsSection;
    [SerializeField] PauseMenuInventorySection _inventorySection;
    [SerializeField] UnsavedSettingsPopup _warningPopUpGO;

    [Header("Overlays")]
    [SerializeField] StatsOverlay _statsOverlay;

    [Header("Other")]
    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _background;
    [SerializeField] GameObject _separatorGroup;
    [SerializeField] GameObject _bottomItemsGroup;
    [SerializeField] PauseMenuSection _currentSection = PauseMenuSection.NONE;
    [SerializeField] MenuWindowState _currentWindowState = MenuWindowState.INACTIVE;
    
    Dictionary<PauseMenuSection, IMenuSection> _sectionsDict;
    Dictionary<MenuOverlay, IMenuOverlay> _overlaysDict;


    void OnEnable()
    {
        _gameStateChangedEventChannel.OnEventRaised += HandleGameStateChanged;
        
    }

    void OnDisable()
    {
        _gameStateChangedEventChannel.OnEventRaised -= HandleGameStateChanged;
    }
    void Awake()
    {
        _sectionsDict = new()
        {
            { PauseMenuSection.MAIN, _mainSection },
            { PauseMenuSection.SETTINGS, _settingsSection },
            { PauseMenuSection.UNSAVED_WARNING, _warningPopUpGO },
            { PauseMenuSection.INVENTORY, _inventorySection}
        };

        _overlaysDict = new()
        {
          { MenuOverlay.STATS, _statsOverlay}  
        };
    }

    void Start()
    {
        SetCanvasActive(false);
    }

    void Update()
    {
        if (PlayerInputManager.Instance.MenuBackInput)
            Execute(MenuAction.Back);
    }

    void HandleGameStateChanged(GameState state)
    {
        switch(state)
        {
            case GameState.PAUSED:
                TransitionToWindowState(MenuWindowState.OPENING);
                break;
            case GameState.PLAYING:
                TransitionToWindowState(MenuWindowState.CLOSING);
                break;
        }
    }

    void TransitionToWindowState(MenuWindowState newState)
    {
        _currentWindowState = newState;
    
        switch (_currentWindowState)
        {
            case MenuWindowState.OPENING:
                BeginOpening();
                break;
    
            case MenuWindowState.ACTIVE:
                //Debug.Log("Pause menu window is open.");
                break;
    
            case MenuWindowState.CLOSING:
                BeginClosing();
                break;
    
            case MenuWindowState.INACTIVE:
                //Debug.Log("Pause Menu window is closed.");
                break;
        }
    }

    void BeginOpening()
    {
        //Debug.Log("Pause Menu window is opening.");
        SetCanvasActive(true);
        SetBackgroundActive(true);
        SetSeparatorsActive(true);
        SetBottomItemsActive(true);

        SetSection(PauseMenuSection.MAIN);
        SetCurrentContext(this);
        _currentWindowState = MenuWindowState.ACTIVE;
        IMenuSection currentSection = _sectionsDict[_currentSection];
        PushSection(currentSection);

        if (_statsOverlay.IsVisible)
            _statsOverlay.RefreshDisplays();
    }

    void BeginClosing()
    {
        
        SetCanvasActive(false);
        SetBackgroundActive(false);
        SetSeparatorsActive(false);
        SetBottomItemsActive(false);

        PopSection();
        SetSection(PauseMenuSection.NONE);
        _currentWindowState = MenuWindowState.INACTIVE;
        UISelector.instance.SetSelected(null);
        Debug.Log("Pause Menu window is closing.");
    }

    void SetSection(PauseMenuSection newSection)
    {
        if (_currentSection != PauseMenuSection.NONE)
            _sectionsDict[_currentSection].Hide();
        
        _currentSection = newSection;

        if (_currentSection != PauseMenuSection.NONE)
            _sectionsDict[_currentSection].Show();
    }

    void ToggleOverlay(MenuOverlay overlay)
    {
        if (_overlaysDict.TryGetValue(overlay, out var o) != true) return;

        if (o.IsVisible)
        {
            Debug.Log("Hiding visible overlay."); 
            o.Hide();
        } 
        
        else 
        {
            Debug.Log("Showing invisible overlay."); 
            o.Show();
        }
    }


    // ----------------------------
    // HELPERS
    // ----------------------------
    void SetCanvasActive(bool enable)
    {
        _canvas.enabled = enable;
    }

    void SetBackgroundActive(bool enable)
    {
        _background.SetActive(enable);
    }

    void SetBottomItemsActive(bool enable)
    {
        _bottomItemsGroup.SetActive(enable);
    }

    void SetSeparatorsActive(bool enable)
    {
        _separatorGroup.SetActive(enable);
    }

    void CloseAllSections()
    {
        foreach (KeyValuePair<PauseMenuSection, IMenuSection> section in _sectionsDict)
        {
            section.Value.Hide();
        }
    }

    public override void Execute(MenuAction action)
    {
        //Debug.Log("Executing menu action : " + action);

        IMenuSection currentSection;
        switch (action)
        {
            case MenuAction.ContinueGame:
                TransitionToWindowState(MenuWindowState.CLOSING);
                GameManager.Instance.SetGameState(GameState.PLAYING);
                break;

            case MenuAction.OpenSettings:
                SetSection(PauseMenuSection.SETTINGS);
                currentSection = _sectionsDict[_currentSection];
                PushSection(currentSection);

                if (_statsOverlay.IsVisible)
                    ToggleOverlay(MenuOverlay.STATS);
                break;

            case MenuAction.Back:
                HandleBack();
                break;

            case MenuAction.ApplySettings:
                SettingsManager.Instance.HandleSettingsSaved();
                break;
            
            case MenuAction.ShowUnsavedWarning:
                SetSection(PauseMenuSection.UNSAVED_WARNING);
                break;

            case MenuAction.WarningConfirmYes:
                OnWarningYes();
                break;

            case MenuAction.WarningConfirmNo:
                OnWarningNo();
                break;

            case MenuAction.ToggleStats:
                ToggleOverlay(MenuOverlay.STATS);
                break;

            case MenuAction.OpenInventory:
                SetSection(PauseMenuSection.INVENTORY);
                currentSection = _sectionsDict[_currentSection];
                PushSection(currentSection);
                break;

            case MenuAction.ReturnToMainMenu:
                GameManager.Instance.SetGameState(GameState.RETURNING_TO_MAIN_MENU);
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
        SetSection(PauseMenuSection.MAIN);
    }

    void OnWarningNo()
    {
        SetSection(PauseMenuSection.SETTINGS);
    }

    void HandleBack()
    {
        IMenuSection currentSection;
        switch (_currentSection)
        {
            case PauseMenuSection.MAIN:
            TransitionToWindowState(MenuWindowState.CLOSING);
            PauseManager.Instance.Resume();
            break;

            case PauseMenuSection.SETTINGS:
                if (SettingsManager.Instance.CheckIfDirty())
                {
                    currentSection = _sectionsDict[_currentSection];
                    PushSection(currentSection);
                    SetSection(PauseMenuSection.UNSAVED_WARNING);
                }
                else
                {
                    PopSection();
                    SetSection(PauseMenuSection.MAIN);
                    //SetSection(PeekSection());
                }
                break;

            case PauseMenuSection.UNSAVED_WARNING:
                OnWarningYes();
            break;

            default:
                PopSection();
                SetSection(PauseMenuSection.MAIN);
                //Debug.Log("Returning to main.");
                break;
        }
    }

}
