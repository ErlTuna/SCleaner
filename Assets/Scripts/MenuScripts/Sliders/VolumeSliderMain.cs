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
        logic.slider.onValueChanged.AddListener(HandleVolumeChange);
        //MainMenuSettingsSection.OnSettingsOpened += SyncSlider;
        SettingsMenu.OnSettingsOpened += SyncSlider;
    }

    void OnDisable()
    {
        logic.slider.onValueChanged.RemoveListener(HandleVolumeChange);
        //MainMenuSettingsSection.OnSettingsOpened -= SyncSlider;
        SettingsMenu.OnSettingsOpened -= SyncSlider;
    }

    void Awake()
    {
        logic.Setup();
    }

    void SyncSlider()
    {
        visuals.UpdateVisuals();
        logic.SyncSlider();
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
        AudioManager.Instance.PlayMenuSubmitSound();
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        MenuAnimationsManager.instance.OptionSelected(visuals.selectionIndicator);
        AudioManager.Instance.PlayMenuSelectSound();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        MenuAnimationsManager.instance.OptionDeselected();
    }

}


