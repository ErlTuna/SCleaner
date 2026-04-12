using UnityEngine;

public class SFXSliderLogic : BaseAudioSliderLogic
{
    public override void Setup()
    {
        slider.wholeNumbers = false;
        slider.minValue = SETTINGS.MIN_VOLUME;
        slider.maxValue = SETTINGS.MAX_VOLUME;
    
        currentValue = SETTINGS.CurrentSFXVolume;
    }

    public override void SyncSlider()
    {
        Debug.Log("SFX Slider is synced.");
        slider.value = SETTINGS.CurrentSFXVolume;
        currentValue = slider.value;
    }

    // Subscribes to slider's onValueChanged event.
    public override void AdjustVolume(float value)
    {
        value = Mathf.Round(value * 10f) / 10f;

        if (Mathf.Abs(value) < 0.001f) value = 0f;

        if (value > slider.maxValue || value < slider.minValue) return;


        currentValue = value;

        SettingsManager.Instance.HandleSFXLevelAltered(currentValue);
    }
}
