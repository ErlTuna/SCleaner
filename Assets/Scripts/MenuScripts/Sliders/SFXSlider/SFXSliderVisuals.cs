
public class SFXSliderVisuals : BaseAudioSliderVisuals
{

    void OnEnable()
    {
        SettingsEvents.OnSettingsReverted += UpdateVisuals;
    }

    void OnDisable()
    {
        SettingsEvents.OnSettingsReverted -= UpdateVisuals;
    }
    public override void UpdateVisuals()
    {
        UnityEngine.Debug.Log("SFX Slider visual update called");
        AudioLevel SFXAudioLevel = AudioManager.instance.SFXLevel;
        volumeBarImage.sprite = volumeBarSprites[(int)SFXAudioLevel];
    }
}
