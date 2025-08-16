using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WarningPopUpYes : MonoBehaviour, IMenuItem
{
    [SerializeField] Image _selectionIndicator;
    [SerializeField] GameObject _transitionFirstSelection;
    public bool suppressEvents = false;
    public void OnDeselect(BaseEventData eventData)
    {
        MenuAnimationsManager.instance.OptionDeselected();
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.instance.PlaySelectSound();
        MenuAnimationsManager.instance.OptionSelected(_selectionIndicator);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (suppressEvents) return;

        suppressEvents = true;
        SettingsManager.instance.HandleSettingsReverted();
        UISelector.instance.SetSelected(_transitionFirstSelection);
        PopUpEvents.RaiseWarningYes();
        suppressEvents = false;
        
    }

    

}
