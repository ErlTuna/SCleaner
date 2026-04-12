
public class SFXSliderVisuals : BaseAudioSliderVisuals
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
        AudioLevel SFXAudioLevel = AudioManager.Instance.SFXLevel;
        volumeBarImage.sprite = volumeBarSprites[(int)SFXAudioLevel];
    }
}
