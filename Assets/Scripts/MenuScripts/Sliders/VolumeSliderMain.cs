using UnityEngine.EventSystems;
using UnityEngine;

public class VolumeSliderMain : MonoBehaviour, ISelectHandler, IDeselectHandler, IDescriptionProvider
{
    public BaseAudioSliderLogic logic;
    public BaseAudioSliderVisuals visuals;
    [SerializeField] string _description;
    public string Label { get; set;}
    public string Description => _description;

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
        AudioManager.Instance.PlaySubmitSound();
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        MenuAnimationsManager.instance.OptionSelected(visuals.selectionIndicator);
        AudioManager.Instance.PlaySelectSound();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        MenuAnimationsManager.instance.OptionDeselected();
    }

}


