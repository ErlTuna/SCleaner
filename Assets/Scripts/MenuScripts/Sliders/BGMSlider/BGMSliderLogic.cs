using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSliderLogic : BaseAudioSliderLogic
{
    public override void Setup()
    {
        slider.wholeNumbers = false;
        slider.minValue = SETTINGS.MIN_VOLUME;
        slider.maxValue = SETTINGS.MAX_VOLUME;

        currentValue = SETTINGS.CurrentBGMVolume;
    }

    public override void SyncSlider()
    {
        slider.value = SETTINGS.CurrentBGMVolume;
        currentValue = slider.value;
    }

    public override void AdjustVolume(float value)
    {
        value = Mathf.Round(value * 10f) / 10f;

        if (Mathf.Abs(value) < 0.001f) value = 0f;

        if (value > slider.maxValue || value < slider.minValue) return;


        currentValue = value;

        SettingsManager.Instance.HandleBGMLevelAltered(currentValue);
    }

}
