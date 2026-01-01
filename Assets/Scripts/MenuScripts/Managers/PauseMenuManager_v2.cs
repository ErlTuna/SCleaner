using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager_v2 : MenuContext
{
    [SerializeField] VoidEventChannelSO _pauseToggleEventChannel;

    [Header("Sections")]
    [SerializeField] PauseMenuMainSection _mainSection;
    [SerializeField] PauseMenuSettingsSection _settingsSection;
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
    
    Dictionary<PauseMenuSection, IMenuSection> _sections;
    Dictionary<MenuOverlay, IMenuOverlay> _overlays;


    protected override void OnEnable()
    {
        base.OnEnable();
        _pauseToggleEventChannel.OnEventRaised += TogglePauseMenu;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _pauseToggleEventChannel.OnEventRaised -= TogglePauseMenu;
    }
    void Awake()
    {
        _sections = new()
        {
            { PauseMenuSection.MAIN, _mainSection },
            { PauseMenuSection.SETTINGS, _settingsSection },
            { PauseMenuSection.UNSAVED_WARNING, _warningPopUpGO },
            { PauseMenuSection.INVENTORY, _inventorySection}
        };

        _overlays = new()
        {
          { MenuOverlay.STATS, _statsOverlay}  
        };
        
    }

    void Start()
    {
        SetCanvasActive(false);
    }

    public void SetWindowState(MenuWindowState newState)
    {
        _currentWindowState = newState;

        switch (_currentWindowState)
        {            
            case MenuWindowState.OPENING:
                Debug.Log("Pause Menu window is opening.");
                OnWindowOpened();
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
                UISelector.instance.SetSelected(null);
                OnWindowClosed();
                SetSection(PauseMenuSection.NONE);
                Debug.Log("Pause Menu window is closed.");
                break;
        }

        
    }

    void OnWindowOpened()
    {
        SetCanvasActive(true);
        SetBackgroundActive(true);
        SetSeparatorsActive(true);
        SetBottomItemsActive(true);
    }

    void OnWindowClosed()
    {
        CloseAllSections();
        SetCanvasActive(false);
        SetBackgroundActive(false);
        SetSeparatorsActive(false);
        SetBottomItemsActive(false);
    }

    void SetSection(PauseMenuSection newSection)
    {
        if (_currentSection != PauseMenuSection.NONE)
            _sections[_currentSection].Hide();

        _currentSection = newSection;

        if (_currentSection != PauseMenuSection.NONE)
            _sections[_currentSection].Show();
    }

    void ToggleOverlay(MenuOverlay overlay)
    {
        if (_overlays.TryGetValue(overlay, out var o) != true) return;
        Debug.Log("Togglings...");

        if (o.IsVisible)
        { 
            o.Hide();
        } 
        
        else 
        {
            Debug.Log("Is it REALLY not visible? " + o.IsVisible);   
            o.Show();
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
        foreach (KeyValuePair<PauseMenuSection, IMenuSection> section in _sections)
        {
            section.Value.Hide();
        }
    }

    public override void Execute(MenuAction action)
    {
        switch (action)
        {
            case MenuAction.ContinueGame:
                GameManager.Instance.SetGameState(GameState.PLAYING);
                break;

            case MenuAction.OpenSettings:
                SetSection(PauseMenuSection.SETTINGS);
                if (_statsOverlay.IsVisible)
                    ToggleOverlay(MenuOverlay.STATS);
                break;

            case MenuAction.Back:
                HandleBack();
                break;

            case MenuAction.ApplySettings:
                SettingsManager.instance.HandleSettingsSaved();
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
                break;
                
            default :
                Debug.Log("The action is not supported for this context.");
                break;
        }
    }

    void OnWarningYes()
    {
        SettingsManager.instance.HandleSettingsReverted();
        SetSection(PauseMenuSection.MAIN);
    }

    void OnWarningNo()
    {
        SetSection(PauseMenuSection.SETTINGS);
    }

    void HandleBack()
    {
        switch (_currentSection)
        {
            case PauseMenuSection.SETTINGS:
                if (SettingsManager.instance.CheckIfAltered())
                {
                    SetSection(PauseMenuSection.UNSAVED_WARNING);
                }
                else
                {
                    SetSection(PauseMenuSection.MAIN);
                }
                break;

            default:
                SetSection(PauseMenuSection.MAIN);
                break;
        }
    }

}
