public class WeaponSFXSliderVisual : BaseAudioSliderVisuals
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
        AudioLevel WeaponSFXAudioLevel = AudioManager.Instance.WeaponSFXLevel;
        volumeBarImage.sprite = volumeBarSprites[(int)WeaponSFXAudioLevel];
    }
}
