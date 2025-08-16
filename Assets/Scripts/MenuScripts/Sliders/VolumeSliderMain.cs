using UnityEngine.EventSystems;
using UnityEngine;

public class VolumeSliderMain : MonoBehaviour, IMenuItem
{
    public BaseAudioSliderLogic logic;
    public BaseAudioSliderVisuals visuals;


    void OnEnable()
    {
        visuals.UpdateVisuals();
        logic.UpdateSliderLogic();
        logic.slider.onValueChanged.AddListener(HandleVolumeChange);
    }

    void OnDisable()
    {
        logic.slider.onValueChanged.RemoveListener(HandleVolumeChange);
    }

    void Awake()
    {
        logic.Setup();
    }


    void HandleVolumeChange(float value)
    {
        if (logic.suppressEvents) return;
        logic.AdjustVolume(value);
        visuals.UpdateVisuals();
        PlayAdjustmentSFX();
    }

    void PlayAdjustmentSFX()
    {
        AudioManager.instance.PlaySubmitSound();
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        MenuAnimationsManager.instance.OptionSelected(visuals.selectionIndicator);
        AudioManager.instance.PlaySelectSound();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        MenuAnimationsManager.instance.OptionDeselected();
    }

    public void OnSubmit(BaseEventData eventData){}
}


