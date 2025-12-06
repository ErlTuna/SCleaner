using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class OptionsReturnButton : MonoBehaviour, IMenuItem
{
    [SerializeField] Image _selectionIndicator;
    public UnityEvent OnSubmittedClean;
    public UnityEvent OnSubmittedDirty;
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
        if (SettingsManager.instance.CheckIfAltered())
        {
            SettingsEvents.RaiseSettingsChangesUnsaved();
            OnSubmittedDirty?.Invoke();
        }
        else
        {
            OnSubmittedClean?.Invoke();
        }

    }

    
}
