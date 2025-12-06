using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuExitOption : MonoBehaviour, IMenuItem
{
    [SerializeField] Image _selectionIndicator;
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
        GameManager.Instance.SetGameState(GameState.SHUTTING_DOWN);
        AudioManager.instance.PlaySubmitSound();
    }
}
