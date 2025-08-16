[System.Serializable]
public class RuntimeSettings
{
    public float BGM_volume;
    public float SFX_volume;
    public bool fullScreen;
    public int resolutionIndex;
    public int minVolume = 0;
    public int maxVolume = 1;

    RuntimeSettings(float BGM_volume, float SFX_volume)
    {
        this.BGM_volume = BGM_volume;
        this.SFX_volume = SFX_volume;
    }

    public RuntimeSettings(){}

    public void CopyFrom(MainMenuDefaultsSO defaults)
    {
        BGM_volume = defaults.BGM_volume;
        SFX_volume = defaults.SFX_volume;
        //fullScreen = defaults.fullScreen;
        //resolutionIndex = defaults.resolutionIndex;
    }

    public RuntimeSettings Clone()
    {
        RuntimeSettings clonedSettings = new RuntimeSettings(BGM_volume, SFX_volume);
        return clonedSettings;
        
    }

}
