using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ContinueButton : MonoBehaviour, IMenuItem
{
    [SerializeField] Image _selectionIndicator;
    bool suppressEvents = false;
    
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
        Debug.Log("Continue Button OnSubmit called");
        if (suppressEvents) return;

        suppressEvents = true;
        AudioManager.instance.PlaySubmitSound();
        MenuOpenCloseEvents.RaisePauseMenuClosed();
        PauseManager.instance.UnPauseGame();
        PlayerInputManager.instance.ToggleMouseInput(true);
        suppressEvents = false;
    }
}
