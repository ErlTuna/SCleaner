using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class WarningPopUpNo : MonoBehaviour, IMenuItem
{
    [SerializeField] Image _selectionIndicator;
    public UnityEvent OnSubmited;

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
        OnSubmited?.Invoke();
    }
}
