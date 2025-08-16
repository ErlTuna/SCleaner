using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsConfirmButton : MonoBehaviour, IMenuItem
{
    public bool suppressEvents = false;
    [SerializeField] Image _selectionIndicator;
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
        SettingsManager.instance.HandleSettingsSaved();
        suppressEvents = false;
    }
}
