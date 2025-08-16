using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour, IMenuItem
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
        AudioManager.instance.PlaySubmitSound();
        MenuAnimationsManager.instance.OptionDeselected();
        SceneManager.LoadScene("Loading_Screen", LoadSceneMode.Single);
    }
}
