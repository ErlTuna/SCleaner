
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
        AudioLevel SFXAudioLevel = AudioManager.Instance.SFXLevel;
        volumeBarImage.sprite = volumeBarSprites[(int)SFXAudioLevel];
    }
}
