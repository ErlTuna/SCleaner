using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MainMenuOptionsButton : MonoBehaviour, IMenuItem
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
