using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuElement : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [SerializeField] Image selectionIndicator;
    [SerializeField] MenuAction action;

    public void OnSelect(BaseEventData eventData)
    {
        MenuAnimationsManager.instance.OptionSelected(selectionIndicator);
        AudioManager.Instance.PlayMenuSelectSound();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        MenuAnimationsManager.instance.OptionDeselected();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlayMenuSubmitSound();
        MenuContext.Current.Execute(action);
    }
}




