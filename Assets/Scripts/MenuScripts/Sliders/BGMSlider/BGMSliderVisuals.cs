public class BGMSliderVisuals : BaseAudioSliderVisuals
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
        AudioLevel BGMAudioLevel = AudioManager.instance.BGMLevel;
        volumeBarImage.sprite = volumeBarSprites[(int)BGMAudioLevel];
    }
}
