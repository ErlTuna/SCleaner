using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler, IDescriptionProvider
{
    [SerializeField] MenuAction _action;
    [SerializeField] Image _selectionIndicator;
    [SerializeField] string _description;
    public string Description => _description;

    public void OnSelect(BaseEventData eventData)
    {
        if (_selectionIndicator != null)
            MenuAnimationsManager.instance.OptionSelected(_selectionIndicator);

        //Debug.Log("Selected me! " + gameObject.name);
        AudioManager.Instance.PlayMenuSelectSound();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (_selectionIndicator != null)
            MenuAnimationsManager.instance.OptionDeselected();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance.PlayMenuSubmitSound();

        if (MenuContext.Current != null)
        {
            MenuContext.Current.Execute(_action);
        }
        else
        {
            Debug.LogWarning($"No active menu context to handle action: {_action}");
        }
    }
}
