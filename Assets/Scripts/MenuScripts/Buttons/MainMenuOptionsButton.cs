using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuOptionsButton : MonoBehaviour, IMenuItem
{
    [SerializeField] Image _selectionIndicator;
    [SerializeField] GameObject _selectionTarget;
    bool _suppressEvents = false;
    
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
        if (_suppressEvents) return;

        _suppressEvents = true;
        AudioManager.instance.PlaySubmitSound();
        MenuOpenCloseEvents.RaiseSettingsOpened();
        UISelector.instance.SetSelected(_selectionTarget);
        _suppressEvents = false;

    }
}
