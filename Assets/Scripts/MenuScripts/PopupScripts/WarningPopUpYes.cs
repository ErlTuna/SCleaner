using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WarningPopUpYes : MonoBehaviour, IMenuItem
{
    [SerializeField] Image _selectionIndicator;
    public UnityEvent OnSubmitted;
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
        OnSubmitted?.Invoke();
    }

    

}
