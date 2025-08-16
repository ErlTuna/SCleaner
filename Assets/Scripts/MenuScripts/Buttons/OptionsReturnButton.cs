using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsReturnButton : MonoBehaviour, IMenuItem
{
    [SerializeField] Image _selectionIndicator;
    [SerializeField] GameObject _mainMenuTransitionFirstSelection;
    [SerializeField] GameObject _settingsUnsavedFirstSelection;

    bool suppressEvents = false;
    public void OnSelect(BaseEventData eventData)
    {
        MenuAnimationsManager.instance.OptionSelected(_selectionIndicator);
        AudioManager.instance.PlaySelectSound();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        MenuAnimationsManager.instance.OptionDeselected();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (suppressEvents == true) return;
        suppressEvents = true;
        if (SettingsManager.instance.CheckIfAltered())
        {
            SettingsEvents.RaiseSettingsChangesUnsaved();
            PopUpEvents.RaisePopUpTriggered();
            UISelector.instance.SetSelected(_settingsUnsavedFirstSelection);
        }
        else
        {
            MenuOpenCloseEvents.RaiseSettingsClosed();
            UISelector.instance.SetSelected(_mainMenuTransitionFirstSelection);
        }
        suppressEvents = false;
    }

    
}
