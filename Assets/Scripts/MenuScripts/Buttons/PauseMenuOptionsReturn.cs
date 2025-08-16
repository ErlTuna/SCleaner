using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuOptionsReturn : MonoBehaviour, IMenuItem
{
    [SerializeField] Image _selectionIndicator;
    [SerializeField] GameObject _returnFirstSelection;
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
            UISelector.instance.SetSelected(_settingsUnsavedFirstSelection);
        }
        else
        {
            MenuOpenCloseEvents.RaiseSettingsClosed();
            UISelector.instance.SetSelected(_returnFirstSelection);
        }
        suppressEvents = false;
    }
}
