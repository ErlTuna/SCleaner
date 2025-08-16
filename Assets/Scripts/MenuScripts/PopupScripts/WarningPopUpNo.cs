using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WarningPopUpNo : MonoBehaviour, IMenuItem
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
        PopUpEvents.RaiseWarningNo();
        UISelector.instance.SetSelected(_transitionFirstSelection);
        suppressEvents = false;
        
    }
}
