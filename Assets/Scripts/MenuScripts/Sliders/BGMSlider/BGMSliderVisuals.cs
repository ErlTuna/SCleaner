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
        AudioLevel BGMAudioLevel = AudioManager.Instance.BGMLevel;
        volumeBarImage.sprite = volumeBarSprites[(int)BGMAudioLevel];
    }
}
