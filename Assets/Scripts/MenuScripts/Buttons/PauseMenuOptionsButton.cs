using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuOptionsButton : MonoBehaviour, IMenuItem
{
    [SerializeField] Image _selectionIndicator;
    public UnityEvent OnSubmitted;
    
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
        AudioManager.instance.PlaySubmitSound();
        OnSubmitted?.Invoke();
    }
}
