public class BGMSliderVisuals : BaseAudioSliderVisuals
{

    void OnEnable()
    {
        SettingsManager.OnChangesReverted += UpdateVisuals;
    }

    void OnDisable()
    {
        SettingsManager.OnChangesReverted -= UpdateVisuals;
    }

    public override void UpdateVisuals()
    {
        AudioLevel BGMAudioLevel = AudioManager.Instance.BGMLevel;
        volumeBarImage.sprite = volumeBarSprites[(int)BGMAudioLevel];
    }
}
