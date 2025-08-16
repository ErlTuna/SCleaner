using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ReturnToMainMenuButton : MonoBehaviour, IMenuItem
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
        if (suppressEvents) return;

        suppressEvents = true;
        AudioManager.instance.PlaySubmitSound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main_Menu_v2");
        suppressEvents = false;
    }
}
